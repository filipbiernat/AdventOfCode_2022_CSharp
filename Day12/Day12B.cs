using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day12
{
    public class Day12B : IDay
    {
        public void Run()
        {
            // You try contacting the Elves using your handheld device, but the river you're following must be too low to get a decent signal.
            // You ask the device for a heightmap of the surrounding area (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day12\Day12.txt").ToList();

            // The heightmap shows the local area from above broken into a grid; the elevation of each square of the grid is given by
            // a single lowercase letter, where a is the lowest elevation, b is the next-lowest, and so on up to the highest elevation, z.
            Dictionary<Coords, char> heightmap = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, char>(
                        new Coords(rowIndex, colIndex), elem)))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Also included on the heightmap are marks for your current position (S) and the location that should get the best signal (E).
            Coords currentPosition = heightmap.FirstOrDefault(elem => elem.Value == 'S').Key;
            Coords destination = heightmap.FirstOrDefault(elem => elem.Value == 'E').Key;

            // Your current position (S) has elevation a, and the location that should get the best signal (E) has elevation z.
            heightmap[currentPosition] = 'a';
            heightmap[destination] = 'z';

            // You'll need to find the shortest path from any square at elevation a to the square marked E.
            Dictionary<Coords, int> stepsToMake = new();
            HashSet<Coords> visitedPositions = new();
            Queue<Coords> positionsToExplore = new();

            stepsToMake[destination] = 0;
            positionsToExplore.Enqueue(destination);

            while (positionsToExplore.Count > 0) // Breadth-first search
            {
                Coords positionToExplore = positionsToExplore.Dequeue();
                if (visitedPositions.Contains(positionToExplore)) continue;
                visitedPositions.Add(positionToExplore);

                // During each step, you can move exactly one square up, down, left, or right. To avoid needing to get out your climbing gear,
                // the elevation of the destination square can be at most one higher than the elevation of your current square.
                List<Coords> nextPossiblePositions = Coords.Direction
                    .Select(direction => direction + positionToExplore)
                    .Where(nextPosition => heightmap.ContainsKey(nextPosition))
                    .Where(nextPosition => heightmap[positionToExplore] <= heightmap[nextPosition] + 1)
                    .ToList();

                nextPossiblePositions.ForEach(nextPossiblePosition =>
                {
                    if (!visitedPositions.Contains(nextPossiblePosition))
                    {
                        positionsToExplore.Enqueue(nextPossiblePosition);
                        stepsToMake[nextPossiblePosition] = stepsToMake[positionToExplore] + 1;
                    }
                });
            }

            // What is the fewest steps required to move starting from any square with elevation a to the location that should get the best signal?
            int output = heightmap
                .Where(positionWithElevation => positionWithElevation.Value == 'a')
                .Select(positionWithElevation => positionWithElevation.Key)
                .Where(positionWithElevationA => stepsToMake.ContainsKey(positionWithElevationA))
                .Select(positionWithElevationA => stepsToMake[positionWithElevationA])
                .Min();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
