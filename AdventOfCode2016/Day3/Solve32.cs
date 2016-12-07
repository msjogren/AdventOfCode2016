using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day3
{
    static class Solve32
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day3\\input3.txt");

            int valid = 0;
            int invalid = 0;

            for (int lineno = 0; lineno < input.Length; lineno += 3)
            {
                string[] lengths1 = input[lineno].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lengths2 = input[lineno+1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lengths3 = input[lineno+2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int[][] sides = new int[3][];

                sides[0] = lengths1.Select(l => int.Parse(l)).ToArray();
                sides[1] = lengths2.Select(l => int.Parse(l)).ToArray();
                sides[2] = lengths3.Select(l => int.Parse(l)).ToArray();

                for (int col = 0; col < 3; col++)
                {
                    if (IsPossibleTriangle(sides[0][col], sides[1][col], sides[2][col]))
                        valid++;
                    else
                        invalid++;
                }
            }

            Console.WriteLine($"Total {input.Length} Possible {valid} Impossible {invalid}");

        }

        private static bool IsPossibleTriangle(int side0, int side1, int side2)
        {
            return
                (side0 + side1) > side2 &&
                (side0 + side2) > side1 &&
                (side1 + side2) > side0;
        }
    }
}
