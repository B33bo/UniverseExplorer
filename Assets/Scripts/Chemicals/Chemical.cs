using System.Collections.Generic;

namespace Universe
{
    public struct Chemical
    {
        public float weight;
        public Ion[] ions;

        public Ion this[int index]
        {
            get => ions[index];
            set => ions[index] = value;
        }

        public Chemical(Ion[] ions)
        {
            weight = 0;
            this.ions = ions;
        }

        //Ion hint: electronsInOuterShell = (electronCount - 2) mod 8
        public Chemical(float weight, string formula)
        {
            this.weight = weight;
            //formula example:
            //NaCl = Sodium Chloride
            //H2O = Dihyrdrogen Monoxide
            //H2+2 = Hydrogen ion with a positive charge (2 electrons lost)

            string[] elements = SplitByElements(formula);

            List<Ion> ions = new List<Ion>(elements.Length);

            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Length == 1)
                {
                    //H
                    ions.Add(Ion.Parse(elements[i]));
                    continue;
                }

                if (elements[i].Length == 2)
                {
                    //He OR H2
                    if (IsLowercaseLetter(elements[i][1]))
                    {
                        //He
                        ions.Add(Ion.Parse(elements[i]));
                        continue;
                    }
                    else
                    {
                        //H2
                        Ion newIon = Ion.Parse(elements[i][0].ToString());
                        byte newCount = (byte)(elements[i][1] - 48);
                        for (int j = 0; j < newCount; j++)
                            ions.Add(newIon);
                        continue;
                    }
                }

                if (elements[i].Length == 0)
                    continue;

                string symbol = elements[i][0].ToString();
                if (IsLowercaseLetter(elements[i][1]))
                    symbol += elements[i][1];

                int indexOfSign = FindSign(elements[i], symbol.Length);

                int count;
                string charge = "";

                if (indexOfSign > 0)
                {
                    //found the charge sign
                    charge = elements[i].Substring(indexOfSign);
                    if (indexOfSign == symbol.Length)
                        count = 1;
                    else
                        count = int.Parse(elements[i].Substring(symbol.Length, indexOfSign - symbol.Length));
                }
                else
                {
                    count = int.Parse(elements[i].Substring(elements.Length));
                }

                Ion ion = Ion.Parse(symbol + charge);
                for (int j = 0; j < count; j++)
                    ions.Add(ion);
            }

            this.ions = ions.ToArray();
        }

        private static int FindSign(string s, int start)
        {
            for (int i = start; i < s.Length; i++)
            {
                if (s[i] == '+' || s[i] == '-')
                    return i;
            }
            return -1;
        }

        private static string[] SplitByElements(string formula)
        {
            List<string> currentFormula = new List<string>();

            for (int i = 0; i < formula.Length; i++)
            {
                if (formula[i] >= 'A' && formula[i] <= 'Z')
                    //uppercase letter
                    currentFormula.Add("");

                currentFormula[currentFormula.Count - 1] += formula[i];
            }

            return currentFormula.ToArray();
        }

        private static bool IsLowercaseLetter(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public override string ToString()
        {
            Dictionary<string, int> chemicalStringValues = new Dictionary<string, int>();
            for (int i = 0; i < ions.Length; i++)
            {
                if (chemicalStringValues.ContainsKey(ions[i].Symbol))
                    chemicalStringValues[ions[i].Symbol]++;
                else
                    chemicalStringValues.Add(ions[i].Symbol, 1);
            }

            string s = "";
            foreach (var item in chemicalStringValues)
            {
                if (item.Value == 1)
                    s += item.Key;
                else
                    s += item.Key + item.Value;
            }

            return s;
        }
    }
}
