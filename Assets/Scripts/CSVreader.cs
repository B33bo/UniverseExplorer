using System;

namespace Universe
{
    public static class CSVreader
    {
        public static T[] GetValues<T>(string[] lines)
        {
            string[][] table = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                table[i] = lines[i].Trim().Split(',');
            }
            return GetValues<T>(table);
        }

        private static T[] GetValues<T>(string[][] table)
        {
            string[] key = table[0];
            Type type = typeof(T);

            T[] values = new T[table.Length - 1];

            for (int i = 1; i < table.Length; i++)
            {
                values[i-1] = (T)GetValue(table[i], key, type);
            }
            return values;
        }

        private static object GetValue(string[] data, string[] key, Type t)
        {
            var newObject = Activator.CreateInstance(t);
            for (int i = 0; i < key.Length; i++)
            {
                var field = t.GetField(key[i]);
                if (field.FieldType == typeof(bool))
                {
                    if (data[i] == "yes")
                    {
                        field.SetValue(newObject, true);
                        continue;
                    }
                    if (data[i] == "no")
                    {
                        field.SetValue(newObject, false);
                        continue;
                    }
                    if (data[i] == "")
                    {
                        field.SetValue(newObject, false);
                        continue;
                    }
                }

                if (field.FieldType.IsEnum)
                {
                    field.SetValue(newObject, Enum.Parse(field.FieldType, data[i]));
                    continue;
                }

                try
                {
                    field.SetValue(newObject, Convert.ChangeType(data[i], field.FieldType));
                }
                catch
                {
                    if (string.IsNullOrEmpty(data[i]))
                    {
                        field.SetValue(newObject, Activator.CreateInstance(field.FieldType)); //default
                        continue;
                    }

                    UnityEngine.Debug.LogError($"CSV: key = {key[i]} value = {data[i]}. Value is not a {field.FieldType}");
                    throw;
                }
            }

            return newObject;
        }
    }
}
