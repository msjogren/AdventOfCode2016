using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace AdventOfCode2016.Day13
{
    class Node
    {
        const int PuzzleInput = 1362;   // 10
        private Point _point;

        public Node(uint x, uint y)
        {
            _point = new Point((int)x, (int)y);
            IsOpenSpace = CalculateIsOpenSpace(x, y);
        }

        public bool IsOpenSpace { get; private set; }
        public bool IsVisited { get; set; }
        public uint X => (uint)_point.X;
        public uint Y => (uint)_point.Y;
        public uint DistanceFromStart { get; set; }
        public Node PreviousNode { get; set; }

        static uint[] hexToBitCount = new uint[] { 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4 };

        static bool CalculateIsOpenSpace(uint x, uint y)
        {
            uint sum = PuzzleInput + x * x + 3 * x + 2 * x * y + y + y * y;
            uint bitsSet = hexToBitCount[sum & 0xF] + hexToBitCount[(sum & 0xF0) >> 4] + hexToBitCount[(sum & 0xF00) >> 8] + hexToBitCount[(sum & 0xF000) >> 12] +
                hexToBitCount[(sum & 0xF0000) >> 16] + hexToBitCount[(sum & 0xF00000) >> 20] + hexToBitCount[(sum & 0xF000000) >> 24] + hexToBitCount[(sum & 0xF0000000) >> 28];
            return bitsSet % 2 == 0;
        }
    }

    static class Solve13
    {
        const uint MazeSize = 60;   // 20


        static Node[,] _maze;

        public static void Solve1()
        {
            _maze = CreateMazeGraph(MazeSize, MazeSize);

            Node to = _maze[31, 39];    // _maze[7, 4]
            uint distance = CalculateDistanceBreadthFirst(_maze[1, 1], to);

            List<Node> path = new List<Node>();
            Node onPath = to;
            while (onPath != null)
            {
                path.Add(onPath);
                onPath = onPath.PreviousNode;
            }

            DrawMaze(path);

            Console.WriteLine();
            Console.WriteLine($"Distance: {distance}");
        }

        public static void Solve2()
        {
            _maze = CreateMazeGraph(MazeSize, MazeSize);

            CalculateDistanceBreadthFirst(_maze[1, 1], null);
            int nodesWithin50Steps = 0;
            for (uint x = 0; x < MazeSize; x++)
            {
                for (uint y = 0; y < MazeSize; y++)
                {
                    if (_maze[x, y].IsVisited && _maze[x, y].DistanceFromStart <= 50)
                    {
                        nodesWithin50Steps++;
                    }
                }
            }

            Console.WriteLine($"Nodes reachable in 50 steps: {nodesWithin50Steps}");
        }

        static uint CalculateDistanceBreadthFirst(Node from, Node to)
        {
            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(from);
            from.PreviousNode = null;
            from.DistanceFromStart = 0;

            while (nodesToVisit.Count > 0)
            {
                Node currentNode = nodesToVisit.Dequeue();
                currentNode.IsVisited = true;

                if (currentNode == to)
                {
                    return to.DistanceFromStart;
                }

                foreach (Node adjacent in EnumeratePossibleAdjacentNodes(currentNode))
                {
                    adjacent.DistanceFromStart = currentNode.DistanceFromStart + 1;
                    adjacent.PreviousNode = currentNode;
                    nodesToVisit.Enqueue(adjacent);
                }
            }

            return uint.MaxValue;
        }

        static IEnumerable<Node> EnumeratePossibleAdjacentNodes(Node current)
        {
            Node next = null;

            if (current.Y < (MazeSize - 1))
            {
                next = _maze[current.X, current.Y + 1];
                if (next.IsOpenSpace && !next.IsVisited) yield return next;
            }
            if (current.X < (MazeSize - 1))
            {
                next = _maze[current.X + 1, current.Y];
                if (next.IsOpenSpace && !next.IsVisited) yield return next;
            }
            if (current.Y > 0)
            {
                next = _maze[current.X, current.Y - 1];
                if (next.IsOpenSpace && !next.IsVisited) yield return next;
            }
            if (current.X > 0)
            {
                next = _maze[current.X - 1, current.Y];
                if (next.IsOpenSpace && !next.IsVisited) yield return next;
            }
        }

        static Node[,] CreateMazeGraph(uint width, uint height)
        {
            Node[,] maze = new Node[width, height];
            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    maze[x, y] = new Node(x, y);
                }
            }

            return maze;
        }

        static void DrawMaze(IList<Node> pathNodes)
        {
            uint width = (uint)_maze.GetLength(0);
            uint height = (uint)_maze.GetLength(1);

            // X axis
            Console.Write("    ");
            for (uint col = 0; col < width; col++) Console.Write(col / 10);
            Console.WriteLine();
            Console.Write("    ");
            for (uint col = 0; col < width; col++) Console.Write(col % 10);
            Console.WriteLine();

            for (uint row = 0; row < height; row++)
            {
                // Y axis
                Console.Write(row.ToString().PadLeft(3));
                Console.Write(' ');
                
                for (uint col = 0; col < width; col++)
                {
                    Node n = _maze[col, row];
                    if (pathNodes.Contains(n))
                    {
                        Console.Write('O');
                    }
                    else
                    {
                        Console.Write(n.IsOpenSpace ? '.' : '#');
                    }
                }
                Console.WriteLine();
            }

        }
    }
}
