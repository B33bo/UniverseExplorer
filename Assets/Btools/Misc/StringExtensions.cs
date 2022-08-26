using UnityEngine;
using System.Collections.Generic;

namespace Btools.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceEscaped(this string Text, Dictionary<char, string> replace)
        {
            if (!replace.ContainsKey('\\'))
                replace.Add('\\', @"\");
            string NewString = "";
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] != '\\')
                {
                    NewString += Text[i];
                    continue;
                }

                if (i == Text.Length - 1)
                {
                    NewString += Text[i];
                    continue;
                }

                if (!replace.ContainsKey(Text[i + 1]))
                {
                    NewString += Text[i];
                    continue;
                }

                NewString += replace[Text[i + 1]];
                i++;
                continue;
            }

            return NewString;
        }

        public static string[] SplitEscaped(this string Text, char splitCharacter)
        {
            if (Text is null)
                return new string[] { "" };
            List<string> words = new List<string>();
            string currentWord = "";

            for (int i = 0; i < Text.Length; i++)
            {
                if (i == Text.Length - 1)
                {
                    if (Text[i] == splitCharacter)
                    {
                        words.Add(currentWord);
                        currentWord = "";
                        break;
                    }
                    currentWord += Text[i];
                    break;
                }

                if (Text[i] == '\\')
                {
                    if (Text[i + 1] == splitCharacter)
                    {
                        currentWord += Text[i + 1];
                        i++;
                        continue;
                    }
                }

                if (Text[i] == splitCharacter)
                {
                    words.Add(currentWord);
                    currentWord = "";
                    continue;
                }
                currentWord += Text[i];
            }
            words.Add(currentWord);
            return words.ToArray();
        }

        public static string Lerp(string a, string b, float t)
        {
            if (t > 1)
                return b;

            int totalCharacters = a.Length + b.Length;
            float t_AfterRemovalOfA = (float)a.Length / totalCharacters;

            float constructLerpT = 1 / t_AfterRemovalOfA;

            if (t > t_AfterRemovalOfA)
            {
                if (t_AfterRemovalOfA == 0)
                    return Construct(b, t);

                return Construct(b, (t - t_AfterRemovalOfA) * constructLerpT);
            }

            return Construct(a, 1 - (constructLerpT * t));
        }

        private static string Construct(string s, float t)
        {
            int charactersToAdd = Mathf.RoundToInt(t * s.Length);
            string newStr = "";

            for (int i = 0; i < charactersToAdd; i++)
                newStr += s[i];

            return newStr;
        }
    }
}
