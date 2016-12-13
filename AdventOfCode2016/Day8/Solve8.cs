using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Day8
{
    static class Solve8
    {
        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day8\\input8.txt");
            const int Width = 50, Height = 6;
            bool[,] screen = new bool[Width, Height];
            bool[] rotated = new bool[Width];

            foreach (string line in input)
            {
                if (line.StartsWith("rect"))
                {
                    string size = line.Replace("rect ", "");
                    int xpos = size.IndexOf('x');
                    int a = int.Parse(size.Substring(0, xpos));
                    int b = int.Parse(size.Substring(xpos + 1));
                    for (int x = 0; x < a; x++)
                    {
                        for (int y = 0; y < b; y++)
                        {
                            screen[x, y] = true;
                        }
                    }
                }
                else if (line.StartsWith("rotate"))
                {
                    int equalsPos = line.IndexOf('=');
                    int rowOrCol = int.Parse(line.Substring(equalsPos + 1, line.IndexOf(' ', equalsPos) - equalsPos - 1));
                    int by = int.Parse(line.Substring(line.LastIndexOf(' ') + 1));

                    if (line.StartsWith("rotate row"))
                    {
                        for (int x = 0; x < Width; x++)
                        { 
                            rotated[(x + by) % Width] = screen[x, rowOrCol];
                        }
                        for (int x = 0; x < Width; x++)
                        {
                            screen[x, rowOrCol] = rotated[x];
                        }
                    }
                    else
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            rotated[(y + by) % Height] = screen[rowOrCol, y];
                        }
                        for (int y = 0; y < Height; y++)
                        {
                            screen[rowOrCol, y] = rotated[y];
                        }
                    }
                }
            }

            int litPixels = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    litPixels += screen[x, y] ? 1 : 0;
                    Console.Write(screen[x, y] ? '*' : ' ');
                }
                Console.WriteLine();
            }

            Console.WriteLine(litPixels);
        }
    }
}
