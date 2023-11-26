using System;
using System.Collections.Generic;

namespace Universe
{
    public partial class RandomNum
    {
        private const int letterFrequencyCount = 444_005;
        private const int consonantTotal = 273812;
        private const int joinerTotal = 149397;
        private const int vowelTotal = 180575;

        // Values tooken from: Frankenstein, Random Archive.org text I accidentally copied, Of mice and men, Lorum ipsum, Wikipedia page on the taylor series, This comment
        private static readonly Dictionary<char, int> letterFrequency = new Dictionary<char, int>() { { 'e', 57977 }, { 't', 38382 }, { 'a', 34411 }, { 'o', 32859 }, { 'n', 31747 }, { 'i', 31445 }, { 's', 26540 }, { 'h', 26164 }, { 'r', 24583 }, { 'd', 21535 }, { 'l', 17438 }, { 'u', 13501 }, { 'm', 12867 }, { 'c', 11443 }, { 'y', 10382 }, { 'w', 10066 }, { 'f', 10058 }, { 'g', 8959 }, { 'p', 7284 }, { 'b', 6542 }, { 'v', 4581 }, { 'k', 3144 }, { 'x', 736 }, { 'q', 405 }, { 'j', 666 }, { 'z', 290 }};

        public static char GetRandomLetter(Random random, string letters, int totalFrequency)
        {
            int rand = random.Next(totalFrequency);
            for (int i = 0; i < letters.Length; i++)
            {
                rand -= letterFrequency[letters[i]];
                if (rand < 0)
                    return letters[i];
            }
            return '!';
        }

        private static char GetConsonant(Random random) => GetRandomLetter(random, "bcdfghjklmnpqrstvwxyz", consonantTotal);

        private static char GetJoiner(Random random) => GetRandomLetter(random, "rsfhlnm", joinerTotal);

        private static char GetVowel(Random random) => GetRandomLetter(random, "aeiouy", vowelTotal);

        private static char GetRandomLetter(Random random) => GetRandomLetter(random, "abcdefghijklmnopqrstuvwxyz", letterFrequencyCount);

        public static string GetWord(int syllables, Random random)
        {
            string word = GetRandomLetter(random).ToString().ToUpper();

            for (int i = 0; i < syllables; i++)
            {
                word += GetVowel(random);

                if (random.Next(0, 2) == 1)
                    word += GetJoiner(random);

                word += GetConsonant(random);
            }

            return word;
        }
    }
}
