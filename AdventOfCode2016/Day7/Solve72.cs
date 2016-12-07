using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day7
{
    static class Solve72
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day7\\input7.txt");
            bool inHypernetSequence;
            int numberOfIPsSupportingSSL = 0;

            foreach (string line in input)
            {
                var hypernets = new List<string>();
                var possibleAbas = new List<string>();

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
                            hypernets.Add(sequence);
                        }
                        else
                        {
                            possibleAbas.AddRange(EnumerateAbas(sequence));
                        }

                        inHypernetSequence = !inHypernetSequence;
                        currentPos = bracketPos + 1;
                        if (bracketPos >= line.Length) break;
                    }
                }

                bool supportsSsl = (from aba in possibleAbas
                                    let bab = new string(new[] { aba[1], aba[0], aba[1] })
                                    from hn in hypernets
                                    where hn.Contains(bab)
                                    select hn).Any();
                if (supportsSsl) numberOfIPsSupportingSSL++;
            }

            Console.WriteLine(numberOfIPsSupportingSSL);
        }

        private static IEnumerable<string> EnumerateAbas(string sequence)
        {
            if (sequence.Length < 3) yield break;
            for (int i = 2; i < sequence.Length; i++)
            {
                if (sequence[i] == sequence[i - 2] && sequence[i] != sequence[i-1])
                {
                    yield return sequence.Substring(i - 2, 3);
                }
            }
        }
    }
}
