using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day4
{
    static class Solve41
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day4\\input4.txt");

            int sum = 0;

            foreach (string line in input)
            {
                int? sectorId = GetSectorIdIfValidRoom(line);
                if (sectorId != null) sum += sectorId.Value;
            }

            Console.WriteLine($"Sum {sum}");
        }

        private static int? GetSectorIdIfValidRoom(string line)
        {
            int lastDash = line.LastIndexOf('-');
            int bracket = line.IndexOf('[', lastDash);
            int sectorId = int.Parse(line.Substring(lastDash + 1, bracket - lastDash - 1));
            string checksum = line.Substring(bracket + 1, line.Length - bracket - 2);
            string encryptedRoomName = line.Remove(lastDash).Replace("-", "");
            //Console.WriteLine($"Sector id {sectorId}, checksum {checksum} name {encryptedRoomName}");

            var counts = new Dictionary<char, int>();
            int currentCount;
            foreach (char c in encryptedRoomName)
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
            
            return top5 == checksum ? sectorId : (int?)null;
        }
    }
}
