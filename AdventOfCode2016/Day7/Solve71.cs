using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day7
{
    static class Solve71
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day7\\input7.txt");
            bool inHypernetSequence;
            int numberOfIPsSupportingTLS = 0;

            foreach (string line in input)
            {
                bool containsAbba = false;
                bool hypernetSequenceContainsAbba = false;

                inHypernetSequence = false;
                int currentPos = 0, bracketPos = 0;
                while (true)
                {
                    bracketPos = line.IndexOf(inHypernetSequence ? ']' : '[', currentPos);
                    if (bracketPos == -1) bracketPos = line.Length;
                    if (bracketPos == 0)
                    {
                        inHypernetSequence = true;
                    }
                    else if (bracketPos > 0)
                    {
                        string sequence = line.Substring(currentPos, bracketPos - currentPos);
                        if (inHypernetSequence)
                        {
                            hypernetSequenceContainsAbba |= ContainsAbba(sequence);
                        }
                        else
                        {
                            containsAbba |= ContainsAbba(sequence);
                        }

                        inHypernetSequence = !inHypernetSequence;
                        currentPos = bracketPos + 1;
                        if (bracketPos >= line.Length) break;
                    }
                }

                if (containsAbba && !hypernetSequenceContainsAbba)
                {
                    numberOfIPsSupportingTLS++;
                }
            }

            Console.WriteLine(numberOfIPsSupportingTLS);
        }

        private static bool ContainsAbba(string sequence)
        {
            if (sequence.Length < 4) return false;
            for (int i = 3; i < sequence.Length; i++)
            {
                if (sequence[i] == sequence[i - 3] && sequence[i-1] == sequence[i-2] && sequence[i] != sequence[i-1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
