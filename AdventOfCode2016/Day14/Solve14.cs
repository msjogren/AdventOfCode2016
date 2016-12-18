using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day14
{
    static class Solve14
    {
        const string PuzzleInput = "cuanljph";
        const bool Part2 = true;
        static MD5 md5Hash = MD5.Create();
        static List<byte[]> _hashes = new List<byte[]>(50000);
        static List<int> _fiveInARowIndices = new List<int>();
        static int _maxFiveIndexChecked = -1;

        static string[] RepeatedThreeTimesStrings = new[] {
            "000", "111", "222", "333", "444", "555", "666", "777",
            "888", "999", "aaa", "bbb", "ccc", "ddd", "eee", "fff"};

        static string[] RepeatedFiveTimesStrings = new[] {
            "00000", "11111", "22222", "33333", "44444", "55555", "66666", "77777",
            "88888", "99999", "aaaaa", "bbbbb", "ccccc", "ddddd", "eeeee", "fffff"};


        public static void Solve()
        {
            int keyCount = 0;

            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            s.Start();

            for (int index = 0; ; index++)
            {
                if (index >= _hashes.Count)
                {
                    CalculateHashUpTo(index + 1000);
                }

                byte[] md5 = _hashes[index];
                int match = FirstDigitFoundThreeInARowInHash(md5);
                if (match >= 0)
                {
                    FindFiveInARowIndicesUpTo(index + 1000);

                    var result =
                        from fiveIndex in _fiveInARowIndices
                        where fiveIndex > index && fiveIndex <= (index + 1000)
                        let md5String = HashToString(_hashes[fiveIndex])
                        where md5String.Contains(RepeatedFiveTimesStrings[match])
                        select new Tuple<int, string>(fiveIndex, md5String);
                        
                    if (result.Any())
                    {
                        keyCount++;
                        Console.WriteLine($"{keyCount}: md5({index}) = {HashToString(md5)} md5({result.First().Item1}) = {result.First().Item2}");
                        if (keyCount >= 64) break;
                    }
                }
            }

            s.Stop();
            Console.WriteLine("Elapsed time (ms): " + s.ElapsedMilliseconds);
        }

        static void CalculateHashUpTo(int toindex)
        {
            for (int index = _hashes.Count; index <= toindex; index++)
            {
                byte[] finalHash = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(PuzzleInput + index));
                if (Part2)
                {
                    for (int i = 0; i < 2016; i++)
                    {
                        finalHash = md5Hash.ComputeHash(HashToAsciiBytes(finalHash));
                    }
                }
                _hashes.Add(finalHash);
            }
        }

        static void FindFiveInARowIndicesUpTo(int toindex)
        {
            CalculateHashUpTo(toindex);

            for (int index = _maxFiveIndexChecked + 1; index <= toindex; index++)
            {
                byte[] md5 = _hashes[index];
                if (HashContainsFiveInARow(md5))
                {
                    _fiveInARowIndices.Add(index);
                }
            }

            _maxFiveIndexChecked = toindex;
        }

        static StringBuilder _hashToStringBuffer = new StringBuilder(32);
        static string HashToString(byte[] hash)
        {
            _hashToStringBuffer.Clear();
            foreach (byte b in hash)
            {
                _hashToStringBuffer.AppendFormat("{0:x2}", b);
            }
            return _hashToStringBuffer.ToString();
        }

        static byte[] AsciiValues = new[] {
            (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7',
            (byte)'8', (byte)'9', (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f'
        };

        static byte[] HashToAsciiBytes(byte[] hash)
        {
            // Effectively the same as Encoding.ASCII.GetBytes(HashToString(hash)) but without string allocation
            byte[] ascii = new byte[hash.Length * 2];
            for (int i = 0; i < hash.Length; i++)
            {
                ascii[2 * i] = AsciiValues[(hash[i] & 0xf0) >> 4];
                ascii[2 * i + 1] = AsciiValues[hash[i] & 0xf];
            }

            return ascii;
        }

        static int FirstDigitFoundThreeInARowInHash(byte[] hash)
        {
            string hashString = HashToString(hash);
            int firstFoundPos = int.MaxValue;
            int result = -1;

            for (int i = 0; i < 16; i++)
            {
                int pos = hashString.IndexOf(RepeatedThreeTimesStrings[i]);
                if (pos >= 0 && pos < firstFoundPos)
                {
                    firstFoundPos = pos;
                    result = i;
                }
            }

            return result;
        }

        static bool HashContainsFiveInARow(byte[] hash)
        {
            string hashString = HashToString(hash);
            for (int i = 0; i < 16; i++)
            {
                if (hashString.Contains(RepeatedFiveTimesStrings[i])) return true;
            }

            return false;
        }
    }
}
