using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day11
{
    public class Day11A : IDay
    {
        public void Run()
        {
            // To get your stuff back, you need to be able to predict where the monkeys will throw your items.
            // After some careful observation, you realize the monkeys operate based on how worried you are about each item.
            // You take some notes (your puzzle input) on the items each monkey currently has, how worried you are about those items,
            List<string> input = File.ReadAllText(@"..\..\..\Day11\Day11.txt").Split("\r\n\r\n").ToList();
            List<Monkey> monkeys = input
                .Select(MonkeyFactory.Build)
                .ToList();

            // After each monkey inspects an item but before it tests your worry level, your relief that the monkey's inspection didn't
            // damage the item causes your worry level to be divided by three and rounded down to the nearest integer.
            static long afterInspection(long worryLevel) => worryLevel / 3;

            // Count the total number of times each monkey inspects items over 20 rounds.
            foreach (int _ in Enumerable.Range(0, 20))
            {
                // The monkeys take turns inspecting and throwing items.
                // Monkey 0 goes first, then monkey 1, and so on until each monkey has had one turn.
                // The process of each monkey taking a single turn is called a round.
                monkeys.ForEach(monkey => monkey.TakeTurn(ref monkeys, afterInspection));
            }

            // Figure out which monkeys to chase by counting how many items they inspect over 20 rounds.
            List<long> inspectionCounters = monkeys
                .Select(monkey => monkey.GetInspectionCounter())
                .OrderByDescending(inspectionCounter => inspectionCounter)
                .ToList();

            // The level of monkey business in this situation can be found by multiplying these together.
            // What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?
            long output = inspectionCounters[0] * inspectionCounters[1];

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
