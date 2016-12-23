using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day22
{
    class Node
    {
        public Node(string name, int size, int used, int avail)
        {
            Name = name;
            Size = size;
            Used = used;
            Avail = avail;
            X = ParseCoordinate('x');
            Y = ParseCoordinate('y');
        }

        public string Name { get; private set; }
        public int Size { get; private set; }
        public int Used { get; private set; }
        public int Avail { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        private int ParseCoordinate(char coord)
        {
            int coordPos = Name.IndexOf(coord);
            int endPos = Name.IndexOf('-', coordPos);
            if (endPos == -1) endPos = Name.Length;
            return int.Parse(Name.Substring(coordPos + 1, endPos - coordPos - 1));
        }
    }

    class SearchState
    {
        public SearchState(int emptyX, int emptyY, int dataX, int dataY)
        {
            Steps = 0;
            EmptySpaceX = emptyX;
            EmptySpaceY = emptyY;
            GoalDataX = dataX;
            GoalDataY = dataY;
            Key = $"({emptyX},{emptyY}),({dataX},{dataY})";
        }

        public SearchState(SearchState previous, int emptyX, int emptyY, int dataX, int dataY) : this(emptyX, emptyY, dataX, dataY)
        {
            Steps = previous.Steps + 1;
        }

        public int Steps { get; private set; }
        public int EmptySpaceX { get; private set; }
        public int EmptySpaceY { get; private set; }

        public int GoalDataX { get; private set; }
        public int GoalDataY { get; private set; }

        public string Key { get; private set; }
    }

    static class Solve22
    {
        static int _gridWidth;
        static int _gridHeight;
        static int _minSize;

        public static void Solve()
        {
            IEnumerable<string> input = File.ReadAllLines("Day22\\input22.txt").Skip(2);
            List<Node> nodes = new List<Node>();

            foreach (string line in input)
            {
                string[] nodeInfo = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                nodes.Add(new Node(nodeInfo[0], ParseSize(nodeInfo[1]), ParseSize(nodeInfo[2]), ParseSize(nodeInfo[3])));
            }

            _gridWidth = nodes.Max(n => n.X) + 1;
            _gridHeight = nodes.Max(n => n.Y) + 1;
            _minSize = nodes.Min(n => n.Size);

            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridHeight; x++)
                {
                    Node node = nodes.First(n => n.X == x && n.Y == y);
                    char c = '.';
                    if (x == 0 && y == 0) c = 'O';
                    if (x == _gridWidth - 1 && y == 0) c = 'G';
                    if (node.Used == 0) c = '_';
                    if (node.Used > _minSize) c = '#';
                    Console.Write($" {c} ");
                }

                Console.WriteLine();
            }

            SolvePart1(nodes);
            SolvePart2(nodes);
        }

        static void SolvePart1(List<Node> nodes)
        {
            int viablePairs = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (j != i && nodes[i].Used > 0 && nodes[i].Used <= nodes[j].Avail)
                    {
                        viablePairs++;
                    }
                }
            }

            Console.WriteLine(viablePairs);
        }

        static void SolvePart2(List<Node> nodes)
        {
            var emptyNodes = nodes.Where(n => n.Used == 0);
            Debug.Assert(emptyNodes.Count() == 1);

            Queue<SearchState> states = new Queue<SearchState>();
            HashSet<string> visitedStates = new HashSet<string>(); 
            SearchState initialState = new SearchState(emptyNodes.First().X, emptyNodes.First().Y, _gridWidth - 1, 0);
            states.Enqueue(initialState);

            while (states.Count > 0)
            {
                SearchState currentState = states.Dequeue();
                if (visitedStates.Contains(currentState.Key)) continue;

                visitedStates.Add(currentState.Key);
                if (currentState.GoalDataX == 0 && currentState.GoalDataY == 0)
                {
                    Console.WriteLine(currentState.Steps);
                    break;
                }

                foreach (Point moveGoalDataTo in GetAdjacentCoordinates(currentState.GoalDataX, currentState.GoalDataY))
                {
                    if (moveGoalDataTo.X == currentState.EmptySpaceX && moveGoalDataTo.Y == currentState.EmptySpaceY)
                    {
                        SearchState newState = new SearchState(currentState, currentState.GoalDataX, currentState.GoalDataY, moveGoalDataTo.X, moveGoalDataTo.Y);
                        if (!visitedStates.Contains(newState.Key)) states.Enqueue(newState);
                        break;
                    }
                }

                foreach (Point moveEmptySpaceTo in GetAdjacentCoordinates(currentState.EmptySpaceX, currentState.EmptySpaceY))
                {
                    if (nodes.Where(n => n.X == moveEmptySpaceTo.X && n.Y == moveEmptySpaceTo.Y).Single().Used <= _minSize &&
                        (moveEmptySpaceTo.X != currentState.GoalDataX || moveEmptySpaceTo.Y != currentState.GoalDataY))
                    {
                        SearchState newState = new SearchState(currentState, moveEmptySpaceTo.X, moveEmptySpaceTo.Y, currentState.GoalDataX, currentState.GoalDataY);
                        if (!visitedStates.Contains(newState.Key)) states.Enqueue(newState);
                    }
                }
            }
        }

        private static IEnumerable<Point> GetAdjacentCoordinates(int x, int y)
        {
            if (x > 0) yield return new Point(x - 1, y);
            if (y > 0) yield return new Point(x, y - 1);
            if (y < (_gridHeight - 1)) yield return new Point(x, y + 1);
            if (x < (_gridWidth - 1)) yield return new Point(x + 1, y);
        }

        private static int ParseSize(string size)
        {
            return int.Parse(size.Remove(size.Length - 1));
        }
    }
}
