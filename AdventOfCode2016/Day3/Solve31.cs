using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day3
{
    static class Solve31
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day3\\input3.txt");

            int valid = 0;
            int invalid = 0;

            foreach (string line in input)
            {
                string[] lengths = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (lengths.Length != 3)
                {
                    invalid++;
                    continue;
                }

                int[] sides = lengths.Select(l => int.Parse(l)).ToArray();

                if (IsPossibleTriangle(sides))
                    valid++;
                else
                    invalid++;
            }

            Console.WriteLine($"Total {input.Length} Possible {valid} Impossible {invalid}");

        }

        private static bool IsPossibleTriangle(int[] sides)
        {
            return
                (sides[0] + sides[1]) > sides[2] &&
                (sides[0] + sides[2]) > sides[1] &&
                (sides[1] + sides[2]) > sides[0];
        }
    }
}
