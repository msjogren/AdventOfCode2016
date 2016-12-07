using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day5
{
    static class Solve51
    {
        public static void Solve()
        {
            string doorId = "ojvtpuvg";
            string password = "";
            long index = 0;
            MD5 md5Hash = MD5.Create();

            while (password.Length < 8)
            {
                byte[] hashBytes = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(doorId + index));
                if (hashBytes[0] == 0 && hashBytes[1] == 0 && hashBytes[2] < 0x10)
                {
                    Console.WriteLine($"Index {index} Hash 0000{hashBytes[2]:x2}{hashBytes[3]:x3}...");
                    password += hashBytes[2].ToString("x");
                }

                index++;
            }

            Console.WriteLine($"Password {password}");
        }
    }
}
