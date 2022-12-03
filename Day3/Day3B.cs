using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day3
{
    public class Day3B : IDay
    {
        public void Run()
        {
            // Each rucksack has two large compartments. All items of a given type are meant to go into exactly one of the two compartments.
            // The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.
            // The Elves have made a list of all of the items currently in each rucksack (your puzzle input).
            // Every item type is identified by a single lowercase or uppercase letter.
            // The list of items for each rucksack is given as characters all on a single line.
            List<string> input = File.ReadAllLines(@"..\..\..\Day3\Day3.txt").ToList();

            // What is the sum of the priorities of those item types?
            int output = FindGroupsOfThreeElves(input)
                .Select(FindTheItemThatAppearsInAllThreeRucksacks)
                .Select(CalculatePriority)
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        // For safety, the Elves are divided into groups of three.
        private static IEnumerable<List<string>> FindGroupsOfThreeElves(List<string> input) => input
            .Select((item, index) => new { item, index })
            .GroupBy(itemWithIndex => itemWithIndex.index / 3, itemWithIndex => itemWithIndex.item)
            .Select(group => group.ToList());

        // Every Elf carries a badge that identifies their group.
        // For efficiency, within each group of three Elves, the badge is the only item type carried by all three Elves.
        private static char FindTheItemThatAppearsInAllThreeRucksacks(List<string> groupsRucksacks) =>
            groupsRucksacks[0]
                .Intersect(groupsRucksacks[1])
                .Intersect(groupsRucksacks[2])
                .First();

        private static int CalculatePriority(char item)
        {
            // To help prioritize item rearrangement, every item type can be converted to a priority:
            // - Lowercase item types a through z have priorities 1 through 26.
            if (item >= 'a') return item - 'a' + 1;
            // - Uppercase item types A through Z have priorities 27 through 52.
            return item - 'A' + 27;
        }
    }
}
