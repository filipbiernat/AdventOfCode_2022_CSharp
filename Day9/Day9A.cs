using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day9
{
    public class Day9A : IDay
    {
        // Consider a rope with a knot at each end; these knots mark the head and the tail of the rope.
        // Assume the head and the tail both start at the same position, overlapping.
        private Coords Head = new(0, 0);
        private Coords Tail = new(0, 0);

        private readonly HashSet<Coords> TailPositions = new();

        public void Run()
        {
            // Due to nebulous reasoning involving Planck lengths, you should be able to model the positions of the knots on a two-dimensional grid.
            // Then, by following a hypothetical series of motions (your puzzle input) for the head, you can determine how the tail will move.
            List<string> input = File.ReadAllLines(@"..\..\..\Day9\Day9.txt").ToList();

            // You just need to work out where the tail goes as the head follows a series of motions.
            TailPositions.Add(Tail);
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
                // After each step, you'll need to update the position of the tail if the step means the head is no longer adjacent to the tail.
                Head += headDelta;
                Tail = MoveTail(Head, Tail);

                TailPositions.Add(Tail);
            }
        }

        private static Coords MoveTail(Coords head, Coords tail)
        {
            // Due to the aforementioned Planck lengths, the rope must be quite short; in fact, the head (H) and tail (T) must always be touching
            // (diagonally adjacent and even overlapping both count as touching).
            if (Coords.Distance(head, tail) > 1)
            {
                // If the head is ever two steps directly up, down, left, or right from the tail,
                // the tail must also move one step in that direction so it remains close enough.
                // Otherwise, if the head and tail aren't touching and aren't in the same row or column,
                // the tail always moves one step diagonally to keep up.
                tail += Coords.DivideAndRoundFurtherFrom0(head - tail, 2);
            }
            return tail;
        }
    }
}

