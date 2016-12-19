using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Day19
{
    class Elf
    {
        public Elf(int number) { Number = number; Presents = 1; }
        public int Number { get; private set; }
        public int Presents { get; set; }
    }

    static class Solve19
    {
        public static void Solve()
        {
            const int PuzzleInput = 3005290;
            const bool Part2 = false;
            LinkedList<Elf> elfs = new LinkedList<Elf>();
            LinkedListNode<Elf> elfToStealFrom = null;

            var firstElfNode = elfs.AddFirst(new Elf(1));
            var previousElfNode = firstElfNode;
            for (int elf = 2; elf <= PuzzleInput; elf++)
            {
                previousElfNode = elfs.AddAfter(previousElfNode, new Elf(elf));
                if ((!Part2 && elf == 2) || (Part2 && (elf == (1 + (PuzzleInput / 2)))))
                {
                    elfToStealFrom = previousElfNode;
                }
            }

            var currentElfNode = firstElfNode;
            do
            {
                currentElfNode.Value.Presents += elfToStealFrom.Value.Presents;

                if (firstElfNode == elfToStealFrom) firstElfNode = firstElfNode.Next;

                LinkedListNode<Elf> nextElfToStealFrom = null;
                if (Part2)
                {
                    nextElfToStealFrom = elfToStealFrom.Next ?? firstElfNode;
                    if ((elfs.Count() % 2) == 1) nextElfToStealFrom = nextElfToStealFrom.Next ?? firstElfNode;
                }
                elfs.Remove(elfToStealFrom);

                currentElfNode = currentElfNode.Next ?? firstElfNode;
                if (Part2)
                {
                    elfToStealFrom = nextElfToStealFrom;
                }
                else
                {
                    elfToStealFrom = currentElfNode.Next ?? firstElfNode;
                }
            } while (currentElfNode.Next != null || currentElfNode.Previous != null);

            Console.WriteLine(currentElfNode.Value.Number);
        }
    }
}
