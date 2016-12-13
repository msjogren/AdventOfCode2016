using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day9
{
    static class Solve92
    {
        public static void Solve()
        {
            string input = File.ReadAllText("Day9\\input9.txt");
            long decompressedLength = DecompressedLengthOfSubstring(input, 0, input.Length);
            Console.WriteLine(decompressedLength);
        }

        static long DecompressedLengthOfSubstring(string input, int start, int length)
        {
            int pos = start;
            long decompressedLength = 0;


            while (pos < (start + length))
            {
                if (input[pos] == '(')
                {
                    int markerStartPos = pos;
                    int markerEndPos = input.IndexOf(')', markerStartPos);
                    string marker = input.Substring(markerStartPos + 1, markerEndPos - markerStartPos - 1);
                    string[] markerComponents = marker.Split('x');
                    int repeatLength = int.Parse(markerComponents[0]);
                    int repeatTimes = int.Parse(markerComponents[1]);
                    decompressedLength += repeatTimes * DecompressedLengthOfSubstring(input, markerEndPos + 1, repeatLength);
                    pos = markerEndPos + repeatLength + 1;
                }
                else
                {
                    decompressedLength++;
                    pos++;
                }
            }

            return decompressedLength;
        }
    }
}
