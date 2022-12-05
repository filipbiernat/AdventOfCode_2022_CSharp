using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day5
{
    public class Day5A : IDay
    {
        public void Run()
        {
            // The Elves don't want to interrupt the crane operator during this delicate procedure, but they forgot to ask her which crate will end up where,
            // and they want to be ready to unload them as soon as possible so they can embark.
            // They do, however, have a drawing of the starting stacks of crates and the rearrangement procedure (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day5\Day5.txt").ToList();
            int lineAfterTheDrawing = input.FindIndex(line => line.Contains(" 1 "));


            IEnumerable<IEnumerable<char>> startingStacksOfCrates = input
                .Take(lineAfterTheDrawing)
                .Select(line => TakeEveryForthItemStartingFromPosition1(line))
                .Reverse();

            List<Stack<char>> stacksOfCrates = Transpose(startingStacksOfCrates)
                .Select(stack => stack.Where(crate => crate != ' '))
                .Select(stacksOfCrates => new Stack<char>(stacksOfCrates))
                .ToList();


            IEnumerable<string> rearrangementProcedure = input.Skip(lineAfterTheDrawing + 2);
            foreach (var step in rearrangementProcedure)
            {
                List<int> extractedNumbers = Regex
                    .Matches(step, @"\d+")
                    .Select(match => int.Parse(match.Value))
                    .ToList();

                int howMany = extractedNumbers[0];
                int moveFrom = extractedNumbers[1] - 1;
                int moveTo = extractedNumbers[2] - 1;

                // Crates are moved one at a time.
                foreach (int crate in Enumerable.Range(0, howMany))
                {
                    char crateToRearrange = stacksOfCrates[moveFrom].Pop();
                    stacksOfCrates[moveTo].Push(crateToRearrange);
                }
            }


            // After the rearrangement procedure completes, what crate ends up on top of each stack?
            string output = new(stacksOfCrates
                .Select(stack => stack.Peek())
                .ToArray());

            Console.WriteLine("Solution: {0}.", output);
        }

        private static IEnumerable<char> TakeEveryForthItemStartingFromPosition1(IEnumerable<char> line) => line.Where((character, index) => index % 4 == 1);
        private static List<List<char>> Transpose(IEnumerable<IEnumerable<char>> matrix) => matrix
            .SelectMany(row => row.Select((item, index) => new { item, index }))
            .GroupBy(elem => elem.index, i => i.item)
            .Select(group => group.ToList())
            .ToList();
    }
}
