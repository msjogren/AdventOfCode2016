using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day9
{
    static class Solve91
    {
        public static void Solve()
        {
            string input = File.ReadAllText("Day9\\input9.txt");

            int pos = 0;
            int decompressedLength = 0;

            while (pos < input.Length)
            {
                if (input[pos] == '(')
                {
                    int markerStartPos = pos;
                    int markerEndPos = input.IndexOf(')', markerStartPos);
                    string marker = input.Substring(markerStartPos + 1, markerEndPos - markerStartPos - 1);
                    string[] markerComponents = marker.Split('x');
                    int repeatLength = int.Parse(markerComponents[0]);
                    int repeatTimes = int.Parse(markerComponents[1]);
                    decompressedLength += repeatTimes * repeatLength;
                    pos = markerEndPos + repeatLength + 1;
                }
                else
                {
                    decompressedLength++;
                    pos++;
                }
            }

            Console.WriteLine(decompressedLength);
        }
    }
}
