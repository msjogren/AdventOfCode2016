using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day16
{
    static class Solve16
    {
        const bool Part2 = true;
        const int DiskSize = Part2 ? 35651584 : 272;
        const string PuzzleInput = "11100010111110100";


        public static void Solve()
        {
            int[] initialData = PuzzleInput.Select(c => c == '1' ? 1 : 0).ToArray();

            IEnumerable<int> randomData = initialData;
            while (randomData.Count() < DiskSize)
            {
                randomData = GenerateRandomLookingData(randomData).ToArray();
            }

            IEnumerable<int> checksumData = randomData.Take(DiskSize);
            while ((checksumData.Count() % 2) == 0)
            {
                checksumData = GenerateChecksum(checksumData).ToArray();
            }

            PrintData(checksumData);
        }

        static IEnumerable<int> GenerateRandomLookingData(IEnumerable<int> input)
        {
            foreach (int i in input)
            {
                yield return i;
            }
            yield return 0;
            foreach (int i in input.Reverse())
            {
                yield return i == 1 ? 0 : 1;
            }
        }

        static IEnumerable<int> GenerateChecksum(IEnumerable<int> input)
        {
            int firstValue = -1;
            bool onFirst = true;

            foreach (int i in input)
            {
                if (onFirst)
                {
                    firstValue = i;
                }
                else
                {
                    yield return firstValue == i ? 1 : 0;
                }
                onFirst = !onFirst;
            }
        }

        static void PrintData(IEnumerable<int> data)
        {
            foreach (int i in data)
            {
                Console.Write(i);
            }
            Console.WriteLine();
        }
    }
}
