using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day18
{
    static class Solve18
    {
        public static void Solve()
        {
            const string PuzzleInput = ".^^.^^^..^.^..^.^^.^^^^.^^.^^...^..^...^^^..^^...^..^^^^^^..^.^^^..^.^^^^.^^^.^...^^^.^^.^^^.^.^^.^.";
            const bool Part2 = true;
            const bool Safe = true; // false => Trap
            const int Rows = Part2 ? 400000 : 40;
            bool[][] tiles = new bool[Rows][];
            int safeTiles = 0;

            tiles[0] = PuzzleInput.ToCharArray().Select(c => c == '.').ToArray();

            safeTiles = tiles[0].Count(c => c == Safe);
            int cols = tiles[0].Length;

            for (int row = 1; row < Rows; row++)
            {
                tiles[row] = new bool[cols];

                for (int col = 0; col < cols; col++)
                {
                    bool leftSafe = col == 0 ? true : tiles[row - 1][col - 1];
                    bool centerSafe = tiles[row - 1][col];
                    bool rightSafe = col == (cols - 1) ? true : tiles[row - 1][col + 1];
                    bool isTrap = 
                        (!leftSafe && !centerSafe && rightSafe) || 
                        (leftSafe && !centerSafe && !rightSafe) || 
                        (!leftSafe && centerSafe && rightSafe) || 
                        (leftSafe && centerSafe && !rightSafe);

                    tiles[row][col] = !isTrap;
                    safeTiles += (isTrap ? 0 : 1);
                }
            }

            Console.WriteLine(safeTiles);
        }
    }
}
