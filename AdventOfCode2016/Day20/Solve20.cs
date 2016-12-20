using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day20
{
    static class Solve20
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day20\\input20.txt");
            List<Tuple<uint, uint>> blockedRanges = new List<Tuple<uint, uint>>();

            foreach (string line in input)
            {
                int dashPos = line.IndexOf('-');
                blockedRanges.Add(new Tuple<uint, uint>(uint.Parse(line.Substring(0, dashPos)), uint.Parse(line.Substring(dashPos + 1))));
            }

            blockedRanges.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));

            // Merge overlapping/adjacent ranges
            bool overlapFound;
            do
            {
                overlapFound = false;
                for (int i = 0; i < (blockedRanges.Count - 1); i++)
                {
                    if (blockedRanges[i].Item2 >= (blockedRanges[i+1].Item1 - 1))
                    {
                        blockedRanges[i] = new Tuple<uint, uint>(blockedRanges[i].Item1, Math.Max(blockedRanges[i].Item2, blockedRanges[i + 1].Item2));
                        blockedRanges.RemoveAt(i + 1);
                        overlapFound = true;
                        break;
                    }
                }
            } while (overlapFound);
            //blockedRanges.ForEach(t => Console.WriteLine($"{t.Item1} - {t.Item2}"));

            // Part 1
            Console.WriteLine(blockedRanges[0].Item1 == 0 ? blockedRanges[0].Item2 + 1 : 0);

            // Part 2
            uint allowedIPs = uint.MaxValue - (uint)blockedRanges.Sum(t => t.Item2 - t.Item1 + 1) + 1;
            Console.WriteLine(allowedIPs);
        }

    }
}
