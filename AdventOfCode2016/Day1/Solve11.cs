using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day1
{
    static class Solve11
    {
        enum Direction
        {
            North,
            East,
            South,
            West
        }

        public static void Solve()
        {
            string input = File.ReadAllText("Day1\\input1.txt");
            var direction = Direction.North;
            int x = 0, y = 0;

            foreach (string instruction in input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var inst = instruction.Trim();
                char rightOrLeft = inst[0];
                int distance = int.Parse(inst.Substring(1));
                direction = Turn(direction, rightOrLeft == 'L');
                switch (direction)
                {
                    case Direction.North:
                        y -= distance;
                        break;
                    case Direction.South:
                        y += distance;
                        break;
                    case Direction.West:
                        x -= distance;
                        break;
                    case Direction.East:
                        x += distance;
                        break;
                }

            }

            Console.WriteLine($"Moved to ({x},{y}), Manhattan distance {Math.Abs(x) + Math.Abs(y)}");
        }

        static Direction Turn(Direction currentDirection, bool turnLeft)
        {
            return (Direction)(((int)currentDirection + (turnLeft ? -1 : 1) + 4) % 4);
        }
    }
}
