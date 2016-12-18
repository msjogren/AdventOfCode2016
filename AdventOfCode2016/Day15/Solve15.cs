using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day15
{
    static class Solve15
    {
        const bool Part2 = true;

        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day15\\input15.txt");

            List<Tuple<int, int>> equations  = new List<Tuple<int, int>>();

            Regex inputPattern = new Regex(@"Disc #(\d+) has (\d+) positions; at time=0, it is at position (\d+).");
            foreach (string line in input)
            {
                Match match = inputPattern.Match(line);

                if (match.Success)
                {
                    equations.Add(new Tuple<int, int>(int.Parse(match.Groups[1].Value) + int.Parse(match.Groups[3].Value), int.Parse(match.Groups[2].Value)));
                }
            }

            if (Part2)
            {
                equations.Add(new Tuple<int, int>(equations.Count + 1 + 0, 11));
            }

            for (int startTime = 0; ; startTime++)
            {
                bool equationsSatisfied = true;
                for (int i = 0; equationsSatisfied && i < equations.Count; i++)
                {
                    var equation = equations[i];
                    // (discNumber + initialPosition + startTime) % numberOfPositions = 0
                    equationsSatisfied &= (((equation.Item1 + startTime) % equation.Item2) == 0);
                }

                if (equationsSatisfied)
                {
                    Console.WriteLine($"Got capsule! Start time {startTime}");
                    break;
                }
            }
        }

    }
}
