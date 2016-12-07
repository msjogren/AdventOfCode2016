using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day2
{
    static class Solve22
    {
        const int ROWS = 5;
        const int COLS = 5;
        const int MAXROW = ROWS - 1;
        const int MAXCOL = COLS - 1;
        static string[,] keypad = new string[,] { { null, null, "1", null, null },
                                                  { null, "2", "3", "4", null },
                                                  { "5", "6", "7", "8", "9" },
                                                  { null, "A", "B", "C", null },
                                                  { null, null, "D", null, null }};

        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day2\\input2.txt");

            int row = 2, col = 0; // Start on 5

            foreach (string line in input)
            {
                foreach (char direction in line)
                {
                    switch (direction)
                    {
                        case 'U':
                            row = (row > 0) && keypad[row - 1, col] != null ? row - 1 : row;
                            break;
                        case 'D':
                            row = (row < MAXROW) && keypad[row + 1, col] != null ? row + 1 : row;
                            break;
                        case 'L':
                            col = (col > 0) && keypad[row, col - 1] != null ? col - 1 : col;
                            break;
                        case 'R':
                            col = (col < MAXCOL) && keypad[row, col + 1] != null ? col + 1 : col;
                            break;
                    }
                }

                Console.WriteLine(keypad[row, col]);
            }

        }
    }
}
