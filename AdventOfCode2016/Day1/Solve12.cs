using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day1
{
    static class Solve12
    {
        enum Direction
        {
            North,
            East,
            South,
            West
        }

        static List<Point> visited = new List<Point>() { };

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
                Point? repeatedVisitPoint = null;

                switch (direction)
                {
                    case Direction.North:
                        repeatedVisitPoint = Visit(new Point(x, y), 0, -distance);
                        y -= distance;
                        break;
                    case Direction.South:
                        repeatedVisitPoint = Visit(new Point(x, y), 0, distance);
                        y += distance;
                        break;
                    case Direction.West:
                        repeatedVisitPoint = Visit(new Point(x, y), -distance, 0);
                        x -= distance;
                        break;
                    case Direction.East:
                        repeatedVisitPoint = Visit(new Point(x, y), distance, 0);
                        x += distance;
                        break;
                }

                if (repeatedVisitPoint != null)
                {
                    Console.WriteLine($"First repeat visit at ({repeatedVisitPoint.Value.X},{repeatedVisitPoint.Value.Y}), Manhattan distance {Math.Abs(repeatedVisitPoint.Value.X) + Math.Abs(repeatedVisitPoint.Value.Y)}");
                    break;
                }
            }
            
        }

        static Point? Visit(Point from, int dx, int dy)
        {
            if (dx != 0)
            {
                for (int cx = from.X; cx != (from.X + dx); cx += Math.Sign(dx))
                {
                    Point pt = new Point(cx, from.Y);
                    if (visited.Contains(pt))
                    {
                        return pt;
                    }
                    else
                    {
                        visited.Add(pt);
                    }
                }

            }

            if (dy != 0)
            {
                for (int cy = from.Y; cy != (from.Y + dy); cy += Math.Sign(dy))
                {
                    Point pt = new Point(from.X, cy);
                    if (visited.Contains(pt))
                    {
                        return pt;
                    }
                    else
                    {
                        visited.Add(pt);
                    }
                }
            }

            return null;
        }

        static Direction Turn(Direction currentDirection, bool turnLeft)
        {
            return (Direction)(((int)currentDirection + (turnLeft ? -1 : 1) + 4) % 4);
        }
    }
}
