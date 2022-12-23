using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day23
{
    public class Day23B : IDay
    {
        public void Run()
        {
            // The Elves each reach into their pack and pull out a tiny plant. The plants rely on important nutrients from the ash, so they can't be planted too close together.
            // There isn't enough time to let the Elves figure out where to plant the seedlings themselves; you quickly scan the grove (your puzzle input) and note their positions.
            List<string> input = File.ReadAllLines(@"..\..\..\Day23\Day23.txt").ToList();

            // The scan shows Elves # and empty ground .; outside your scan, more empty ground extends a long way in every direction.
            HashSet<Coords> initialPositionsOfTheElves = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, char>(
                        new Coords(rowIndex, colIndex), elem)))
                .Where(pair => pair.Value == '#')
                .Select(pair => pair.Key)
                .ToHashSet();

            // The Elves follow a time-consuming process to figure out where they should each go; you can speed up this process considerably.
            // The process consists of some number of rounds during which Elves alternate between considering where to move and actually moving.
            // Finish simulating the process and figure out where the Elves need to go.
            PositionsOfTheElves positionsOfTheElves = new(initialPositionsOfTheElves);
            int numOfTheFirstRoundWhereNoElfMoves =  positionsOfTheElves.SimulateRounds();

            // What is the number of the first round where no Elf moves?
            int output = numOfTheFirstRoundWhereNoElfMoves;

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
