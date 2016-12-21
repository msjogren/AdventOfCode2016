using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day21
{
    static class Solve21
    {
        public static void Solve()
        {
            const bool Part2 = true;
            IEnumerable<string> input = File.ReadAllLines("Day21\\input21.txt");
            const string PasswordToScramble = Part2 ? "fbgdceah" : "abcdefgh";

            if (Part2)
            {
                input = input.Reverse();
            }

            char[] scrambled = PasswordToScramble.ToCharArray();

            foreach (string line in input)
            {
                string[] operation = line.Split(' ');

                if (operation[0] == "swap" && operation[1] == "position")
                {
                    int x = int.Parse(operation[2]);
                    int y = int.Parse(operation[5]);
                    char tmp = scrambled[x];
                    scrambled[x] = scrambled[y];
                    scrambled[y] = tmp;
                }
                else if (operation[0] == "swap" && operation[1] == "letter")
                {
                    char x = operation[2][0];
                    char y = operation[5][0];

                    for (int i = 0; i < scrambled.Length; i++)
                    {
                        if (scrambled[i] == x)
                        {
                            scrambled[i] = y;
                        }
                        else if (scrambled[i] == y)
                        {
                            scrambled[i] = x;
                        }
                    }
                }
                else if (operation[0] == "rotate" && (operation[1] == "left" || operation[1] == "right"))
                {
                    char[] rotated = new char[scrambled.Length];
                    int steps = int.Parse(operation[2]) % scrambled.Length;
                    
                    for (int i = 0; i < scrambled.Length; i++)
                    {
                        if ((!Part2 && operation[1] == "right") || (Part2 && operation[1] == "left"))
                        {
                            rotated[(i + steps) % scrambled.Length] = scrambled[i];
                        }
                        else
                        {
                            rotated[i] = scrambled[(i + steps) % scrambled.Length];
                        }
                    }

                    scrambled = rotated;
                }
                else if (operation[0] == "rotate" && operation[1] == "based")
                {
                    char[] rotated = new char[scrambled.Length];
                    char x = operation[6][0];
                    int stepsToRotateRight;

                    if (Part2)
                    {
                        int posAfterRotation = Array.IndexOf<char>(scrambled, x);
                        int posBeforeRotation;
                        for (posBeforeRotation = 0; posBeforeRotation < scrambled.Length; posBeforeRotation++)
                        {
                            if ((posBeforeRotation + posBeforeRotation + (posBeforeRotation >= 4 ? 2 : 1)) % scrambled.Length == posAfterRotation)
                            {
                                break;
                            }
                        }
                     
                        stepsToRotateRight = (scrambled.Length + posBeforeRotation - posAfterRotation) % scrambled.Length;
                    }
                    else
                    { 
                        stepsToRotateRight = Array.IndexOf<char>(scrambled, x);
                        stepsToRotateRight += (stepsToRotateRight >= 4 ? 2 : 1);
                        stepsToRotateRight %= scrambled.Length;
                    }

                    for (int i = 0; i < scrambled.Length; i++)
                    {
                        rotated[(i + stepsToRotateRight) % scrambled.Length] = scrambled[i];
                    }
                    scrambled = rotated;
                }
                else if (operation[0] == "reverse")
                {
                    int x = int.Parse(operation[2]);
                    int y = int.Parse(operation[4]);
                    char[] tmp = new char[y - x + 1];
                    for (int i = x, j = 0; i <= y; i++, j++)
                    {
                        tmp[j] = scrambled[i];
                    }
                    for (int i = y, j = 0; i >= x; i--, j++)
                    {
                        scrambled[i] = tmp[j];
                    }
                }
                else if (operation[0] == "move")
                {
                    int x = int.Parse(operation[Part2 ? 5 : 2]);
                    int y = int.Parse(operation[Part2 ? 2 : 5]);
                    List<char> tmp = new List<char>(scrambled);
                    char c = tmp[x];
                    tmp.RemoveAt(x);
                    tmp.Insert(y, c);
                    scrambled = tmp.ToArray();
                }
            }

            Console.WriteLine(new string(scrambled));
        }

    }
}
