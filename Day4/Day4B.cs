using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day4
{
    public class Day4B : IDay
    {
        public void Run()
        {
            // Every section has a unique ID number, and each Elf is assigned a range of section IDs.
            // To try to quickly find overlaps and reduce duplicated effort, the Elves pair up and make a big list of the section assignments for each pair (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day4\Day4.txt").ToList();

            // In how many assignment pairs does one range fully contain the other?
            int output = input
                .Select(pairOfAssignments => pairOfAssignments
                    .Split(',')
                    .Select(assignment => new Assignment(assignment)))
                .Count(pairOfAssignments => pairOfAssignments
                    .Last()
                    .Overlaps(pairOfAssignments.First()));

            Console.WriteLine("Solution: {0}.", output);
        }

        private class Assignment
        {
            private readonly int LowestSection, HighestSection;

            public Assignment(string input)
            {
                string[] range = input.Split('-');
                LowestSection = int.Parse(range[0]);
                HighestSection = int.Parse(range[1]);
            }

            public bool Overlaps(Assignment other) => HighestSection >= other.LowestSection && LowestSection <= other.HighestSection;
        }
    }
}
