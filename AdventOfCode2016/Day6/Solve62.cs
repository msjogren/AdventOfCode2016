using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day6
{
    static class Solve62
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day6\\input6.txt");
            Dictionary<char, int>[] characterCounts = new Dictionary<char, int>[input[0].Length];

            for (int i = 0; i < characterCounts.Length; i++)
            {
                characterCounts[i] = new Dictionary<char, int>();
            }

            foreach (string line in input)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char ch = line[i];
                    int count;
                    characterCounts[i][ch] = characterCounts[i].TryGetValue(ch, out count) ? count + 1 : 1;
                }
            }

            foreach (var dict in characterCounts)
            {
                char leastCommon = (from kvp in dict orderby kvp.Value ascending select kvp.Key).First();
                Console.Write(leastCommon);
            }

            Console.WriteLine();
        }
    }
}
