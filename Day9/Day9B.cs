using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day9
{
    public class Day9B : IDay
    {
        // Rather than two knots, you now must simulate a rope consisting of ten knots.
        private List<Coords> Knots = Enumerable.Range(0, 10).Select(_ => new Coords(0, 0)).ToList();

        private readonly HashSet<Coords> TailPositions = new();

        public void Run()
        {
            // Due to nebulous reasoning involving Planck lengths, you should be able to model the positions of the knots on a two-dimensional grid.
            // Then, by following a hypothetical series of motions (your puzzle input) for the head, you can determine how the tail will move.
            List<string> input = File.ReadAllLines(@"..\..\..\Day9\Day9.txt").ToList();

            // You just need to work out where the tail goes as the head follows a series of motions.
            TailPositions.Add(Knots.Last());
            input.ForEach(MakeAStep);

            // How many positions does the tail of the rope visit at least once?
            int output = TailPositions.Count;

            Console.WriteLine("Solution: {0}.", output);
        }

        private void MakeAStep(string instruction)
        {
            string[] splitInstruction = instruction.Split(' ');
            Coords headDelta = Coords.Direction[splitInstruction.First()];
            int numOfSteps = int.Parse(splitInstruction.Last());

            foreach (int _ in Enumerable.Range(0, numOfSteps))
            {
                // One knot is still the head of the rope and moves according to the series of motions.
                List<Coords> newKnots = new() { Knots.First() + headDelta };

                // Each knot further down the rope follows the knot in front of it using the same rules as before.
                Knots
                    .Skip(1)
                    .ToList()
                    .ForEach(knot => newKnots.Add(MoveKnot(newKnots.Last(), knot)));
                Knots = newKnots;

                TailPositions.Add(Knots.Last());
            }
        }

        private static Coords MoveKnot(Coords precedingKnot, Coords knot)
        {
            // Due to the aforementioned Planck lengths, the rope must be quite short; in fact, the head (H) and tail (T) must always be touching
            // (diagonally adjacent and even overlapping both count as touching).
            if (Coords.Distance(precedingKnot, knot) > 1)
            {
                // If the head is ever two steps directly up, down, left, or right from the tail,
                // the tail must also move one step in that direction so it remains close enough.
                // Otherwise, if the head and tail aren't touching and aren't in the same row or column,
                // the tail always moves one step diagonally to keep up.
                knot += Coords.DivideAndRoundFurtherFrom0(precedingKnot - knot, 2);
            }
            return knot;
        }
    }
}

