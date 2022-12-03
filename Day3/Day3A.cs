using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day3
{
    public class Day3A : IDay
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
            int output = input
                .Select(FindTheItemThatAppearsInBothCompartments)
                .Select(CalculatePriority)
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        private static char FindTheItemThatAppearsInBothCompartments(string itemsInRucksack)
        {
            // A given rucksack always has the same number of items in each of its two compartments.
            // The first half of the characters represent items in the first compartment.
            IEnumerable<char> itemsInCompartment1 = itemsInRucksack.Take(itemsInRucksack.Length / 2);
            // The second half of the characters represent items in the second compartment.
            IEnumerable<char> itemsInCompartment2 = itemsInRucksack.Skip(itemsInRucksack.Length / 2);
            // Find the item type that appears in both compartments of each rucksack.
            return itemsInCompartment1
                .Intersect(itemsInCompartment2)
                .First();
        }

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
