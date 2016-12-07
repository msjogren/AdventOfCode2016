using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day4
{
    static class Solve42
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day4\\input4.txt");

            foreach (string line in input)
            {
                string decryptedName = GetDecryptedNameOfValidRoom(line);
                if (decryptedName != null && decryptedName.Contains("north"))
                {
                    Console.WriteLine(decryptedName);
                }
            }
        }

        private static string GetDecryptedNameOfValidRoom(string line)
        {
            int lastDash = line.LastIndexOf('-');
            int bracket = line.IndexOf('[', lastDash);
            int sectorId = int.Parse(line.Substring(lastDash + 1, bracket - lastDash - 1));
            string checksum = line.Substring(bracket + 1, line.Length - bracket - 2);
            string encryptedRoomName = line.Remove(lastDash);

            var counts = new Dictionary<char, int>();
            int currentCount;
            foreach (char c in encryptedRoomName.Replace("-", ""))
            {
                if (counts.TryGetValue(c, out currentCount))
                {
                    counts[c] = ++currentCount;
                }
                else
                {
                    counts[c] = 1;
                }
            }

            string top5 = new String((from kvp in counts orderby kvp.Value descending, kvp.Key ascending select kvp.Key).Take(5).ToArray());
            bool isValid = top5 == checksum;
            if (isValid)
            {
                char[] chars = encryptedRoomName.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i] == '-')
                    {
                        chars[i] = ' ';
                    }
                    else
                    {
                        for (int j = 0; j < sectorId; j++)
                        {
                            if (chars[i] == 'z')
                            {
                                chars[i] = 'a';
                            }
                            else
                            {
                                chars[i]++;
                            }
                        }
                    }
                }

                return new string(chars) + " " + sectorId;
            }

            return null;
        }
    }
}
