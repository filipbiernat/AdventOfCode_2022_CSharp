using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day11
{
    public class Day11B : IDay
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

            // You're worried you might not ever get your items back.
            // So worried, in fact, that your relief that a monkey's inspection didn't damage an item no longer causes your worry level to be divided by three.
            // Unfortunately, that relief was all that was keeping your worry levels from reaching ridiculous levels.
            // You'll need to find another way to keep your worry levels manageable.
            long leastCommonMultiple = FindLeastCommonMultiple(monkeys.Select(monkey => monkey.GetDivisableBy()).ToList());
            long afterInspection(long worryLevel) => worryLevel % leastCommonMultiple; // TRICK: Modulo LCM after each inspection.

            // At this rate, you might be putting up with these monkeys for a very long time - possibly 10000 rounds!
            foreach (int _ in Enumerable.Range(0, 10000))
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
            // Starting again from the initial state in your puzzle input, what is the level of monkey business after 10000 rounds?
            long output = inspectionCounters[0] * inspectionCounters[1];

            Console.WriteLine("Solution: {0}.", output);
        }

        private static long FindLeastCommonMultiple(List<long> numbers) => numbers.Aggregate((lhs, rhs) => lhs * rhs / FindGreatestCommonDivisor(lhs, rhs));
        private static long FindGreatestCommonDivisor(long lhs, long rhs) => rhs == 0 ? lhs : FindGreatestCommonDivisor(rhs, lhs % rhs);
    }
}
