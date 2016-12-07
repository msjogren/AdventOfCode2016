using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day2
{
    static class Solve21
    {
        const int ROWS = 3;
        const int COLS = 3;
        const int MAXROW = ROWS - 1;
        const int MAXCOL = COLS - 1;
        static int[,] keypad = new int[ROWS,COLS] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };

        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day2\\input2.txt");

            int row = 1, col = 1; // Start on 5

            foreach (string line in input)
            {
                foreach (char direction in line)
                {
                    switch (direction)
                    {
                        case 'U':
                            row = (row > 0) ? row - 1 : 0;
                            break;
                        case 'D':
                            row = (row >= MAXROW) ? MAXROW : row + 1;
                            break;
                        case 'L':
                            col = (col > 0) ? col - 1 : 0;
                            break;
                        case 'R':
                            col = (col >= MAXCOL) ? MAXCOL : col + 1;
                            break;
                    }
                }

                Console.WriteLine(keypad[row, col]);
            }

        }
    }
}
