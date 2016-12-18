using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day17
{
    enum Direction
    {
        U,
        D,
        L,
        R
    }

    class Node
    {
        const int GridWidth = 4;
        const int GridHeight = GridWidth;
        const string PuzzleInput = "veumntbg";
        static MD5 md5Hash = MD5.Create();
        private byte[] _hash;

        public static readonly Node StartNode = new Node();

        private Node()
        {
            X = Y = 0;
            Path = "";
            CalculateHash();
        }

        public Node(Node previous, Direction directionFromPrevious)
        {
            switch (directionFromPrevious)
            {
                case Direction.U:
                    X = previous.X;
                    Y = previous.Y - 1;
                    break;

                case Direction.D:
                    X = previous.X;
                    Y = previous.Y + 1;
                    break;

                case Direction.L:
                    X = previous.X - 1;
                    Y = previous.Y;
                    break;

                case Direction.R:
                    X = previous.X + 1;
                    Y = previous.Y;
                    break;
            }

            Path = previous.Path + directionFromPrevious.ToString();
            CalculateHash();
        }

        private void CalculateHash()
        {
            _hash = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(PuzzleInput + Path));
        }

        private static bool IsOpen(int doorHash)
        {
            return doorHash >= 0xb && doorHash <= 0xf;
        }

        public IReadOnlyList<Direction> GetPossibleDirections()
        {
            List<Direction> directions = new List<Direction>();
            if (IsOpen(_hash[0] & 0xf) && Y < (GridHeight - 1)) directions.Add(Direction.D);
            if (IsOpen((_hash[0] & 0xf0) >> 4) && Y > 0) directions.Add(Direction.U);
            if (IsOpen(_hash[1] & 0xf) && X < (GridWidth - 1)) directions.Add(Direction.R);
            if (IsOpen((_hash[1] & 0xf0) >> 4) && X > 0) directions.Add(Direction.L);

            return directions;
        }

        public bool IsVaultRoom { get { return X == GridWidth - 1 && Y == GridHeight - 1; } }

        public uint X { get; private set; }
        public uint Y { get; private set; }
        public string Path { get; private set; }
    }

    static class Solve17
    {
        public static void Solve()
        {
            Stack<Node> nodes = new Stack<Node>();
            List<string> solutionPaths = new List<string>();

            nodes.Push(Node.StartNode);

            while (nodes.Count > 0)
            {
                Node currentNode = nodes.Pop();

                if (currentNode.IsVaultRoom)
                {
                    solutionPaths.Add(currentNode.Path);
                }
                else
                {
                    foreach (Direction dir in currentNode.GetPossibleDirections())
                    {
                        nodes.Push(new Node(currentNode, dir));
                    }
                }
            }

            solutionPaths.Sort((s1, s2) => s1.Length.CompareTo(s2.Length));
            string shortest = solutionPaths[0];
            string longest = solutionPaths.Last();
            Console.WriteLine($"{shortest}, {longest.Length}");
        }
    }
}
