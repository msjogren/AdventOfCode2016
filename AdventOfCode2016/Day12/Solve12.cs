using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Day12
{
    static class Solve12
    {
        private static Dictionary<string, int> _registers;

        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day12\\input12.txt");
            char[] sep = new[] { ' ' };
            string[][] asm = input.Select(line => line.Split(sep, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            int ip = 0;

            _registers = new Dictionary<string, int>() { { "a", 0 }, { "b", 0 }, { "c", /*0*/ 1 }, { "d", 0 } };

            while (ip < asm.Length)
            {
                switch (asm[ip][0])
                {
                    case "cpy":
                        _registers[asm[ip][2]] = Resolve(asm[ip][1]);
                        ip++;
                        break;

                    case "inc":
                        _registers[asm[ip][1]] = _registers[asm[ip][1]] + 1;
                        ip++;
                        break;

                    case "dec":
                        _registers[asm[ip][1]] = _registers[asm[ip][1]] - 1;
                        ip++;
                        break;

                    case "jnz":
                        if (Resolve(asm[ip][1]) != 0)
                        {
                            ip += int.Parse(asm[ip][2]);
                        }
                        else
                        {
                            ip++;
                        }
                        break;
                }

            }

            Console.WriteLine(_registers["a"]);
        }

        static int Resolve(string regOrValue)
        {
            int value;
            return int.TryParse(regOrValue, out value) ? value : _registers[regOrValue];
        }
    }
}
