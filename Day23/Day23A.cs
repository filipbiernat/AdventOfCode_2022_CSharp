using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day23
{
    public class Day23A : IDay
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
            // To make sure they're on the right track, the Elves like to check after round 10 that they're making good progress toward covering enough ground.
            PositionsOfTheElves positionsOfTheElves = new(initialPositionsOfTheElves);
            positionsOfTheElves.SimulateRounds(10);

            // Count the number of empty ground tiles contained by the smallest rectangle that contains every Elf.
            HashSet<Coords> finalPositionsOfTheElves = positionsOfTheElves.Get();
            Coords topLeftCornerOfTheSmallestRectange = new(finalPositionsOfTheElves.Min(coords => coords.Row), finalPositionsOfTheElves.Min(coords => coords.Column));
            Coords bottomRightCornerOfTheSmallestRectange = new(finalPositionsOfTheElves.Max(coords => coords.Row), finalPositionsOfTheElves.Max(coords => coords.Column));
            int smallestRectangeArea = Coords.CalculateRectangeArea(topLeftCornerOfTheSmallestRectange, bottomRightCornerOfTheSmallestRectange);
            int numOfEmptyGroundTiles = smallestRectangeArea - finalPositionsOfTheElves.Count;

            // How many empty ground tiles does that rectangle contain?
            int output = numOfEmptyGroundTiles;

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
