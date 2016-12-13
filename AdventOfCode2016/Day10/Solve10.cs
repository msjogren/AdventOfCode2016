using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Day10
{
    class ChipHolder
    {
        protected List<int> _chips = new List<int>();
        protected int _number;

        public IReadOnlyList<int> Chips => _chips;

        public ChipHolder(int number)
        {
            _number = number;
        }

        public void ReceiveChip(int chipValue)
        {
            _chips.Add(chipValue);
        }

    }

    class Bot : ChipHolder
    {
        public Bot(int botNumber) : base(botNumber) { }

        public bool GiveChips()
        {
            if (_chips.Count == 2 && LowChipReceiver != null && HighChipReceiver != null)
            {
                Console.WriteLine($"Bot {_number} compares {HighChipValue.Value} with {LowChipValue.Value}");
                LowChipReceiver.ReceiveChip(LowChipValue.Value);
                HighChipReceiver.ReceiveChip(HighChipValue.Value);
                _chips.Clear();
                return true;
            }

            return false;
        }

        public ChipHolder LowChipReceiver { get; set; }
        public ChipHolder HighChipReceiver { get; set; }
        public int? LowChipValue { get { return _chips.Any() ? _chips.Min() : (int?)null; } }
        public int? HighChipValue { get { return _chips.Any() ? _chips.Max() : (int?)null; } }

    }

    static class Solve10
    {
        private static Dictionary<int, Bot> _bots = new Dictionary<int, Bot>();
        private static Dictionary<int, ChipHolder> _outputs = new Dictionary<int, ChipHolder>();

        private static Bot GetBot(int botNumber)
        {
            Bot bot;
            if (!_bots.TryGetValue(botNumber, out bot))
            {
                bot = new Bot(botNumber);
                _bots.Add(botNumber, bot);
            }
            return bot;
        }
        private static ChipHolder GetOutput(int outputNumber)
        {
            ChipHolder output;
            if (!_outputs.TryGetValue(outputNumber, out output))
            {
                output = new ChipHolder(outputNumber);
                _outputs.Add(outputNumber, output);
            }
            return output;
        }

        public static void Solve()
        {
            string[] input = File.ReadAllLines("Day10\\input10.txt");

            Regex assignmentPattern = new Regex(@"value (\d+) goes to bot (\d+)");
            Regex givingPattern = new Regex(@"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)");

            foreach (string line in input)
            {
                Match givingMatch = givingPattern.Match(line);

                if (givingMatch.Success)
                {
                    Bot giverBot = GetBot(int.Parse(givingMatch.Groups[1].Value));
                    giverBot.LowChipReceiver = givingMatch.Groups[2].Value == "bot" ? GetBot(int.Parse(givingMatch.Groups[3].Value)) : GetOutput(int.Parse(givingMatch.Groups[3].Value));
                    giverBot.HighChipReceiver = givingMatch.Groups[4].Value == "bot" ? GetBot(int.Parse(givingMatch.Groups[5].Value)) : GetOutput(int.Parse(givingMatch.Groups[5].Value));
                }
                else
                {
                    Match assignmentMatch = assignmentPattern.Match(line);
                    Debug.Assert(assignmentMatch.Success);
                    Bot receiver = GetBot(int.Parse(assignmentMatch.Groups[2].Value));
                    receiver.ReceiveChip(int.Parse(assignmentMatch.Groups[1].Value));
                }
            }

            bool anyChipsChangedOwner;
            do
            {
                anyChipsChangedOwner = false;
                Console.WriteLine();

                foreach (Bot bot in _bots.Values)
                {
                    anyChipsChangedOwner |= bot.GiveChips();
                }
            } while (anyChipsChangedOwner);


            Console.Write(_outputs[0].Chips.Aggregate(Multiply) * _outputs[1].Chips.Aggregate(Multiply) * _outputs[2].Chips.Aggregate(Multiply));
        }

        static int Multiply(int i, int j)
        {
            return i * j;
        }
    }
}
