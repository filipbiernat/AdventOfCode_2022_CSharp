using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day14
{
    public class Day14B : IDay
    {
        public void Run()
        {
            // You scan a two-dimensional vertical slice of the cave above you (your puzzle input) and discover that it is mostly air with structures made of rock.
            List<string> input = File.ReadAllLines(@"..\..\..\Day14\Day14.txt").ToList();

            // Your scan traces the path of each solid rock structure and reports the x,y coordinates that form the shape of the path, where x represents
            // distance to the right and y represents distance down. Each path appears as a single line of text in your scan.
            HashSet<Coords> rock = input
                .SelectMany(rockStructure => ParseRockStructure(rockStructure))
                .ToHashSet();

            // The sand is pouring into the cave from point 500,0.
            Coords sourceOfSand = new(500, 0);
            HashSet<Coords> sand = new();

            // You don't have time to scan the floor, so assume the floor is an infinite horizontal line
            // with a y coordinate equal to two plus the highest y coordinate of any point in your scan.
            int bottomY = rock.Max(coords => coords.Y) + 2;

            // Using your scan, simulate the falling sand until the source of the sand becomes blocked.
            while (!sand.Contains(sourceOfSand))
            {
                // Sand is produced one unit at a time, and the next unit of sand is not produced until the previous unit of sand comes to rest.
                // A unit of sand is large enough to fill one tile of air in your scan.
                Coords unitOfSand = sourceOfSand;

                // Sand keeps moving as long as it is able to do so, at each step trying to move down, then down-left, then down-right.
                while (true)
                {
                    // A unit of sand always falls down one step if possible.
                    Coords down = unitOfSand + Coords.Direction["down"];
                    if (!(rock.Contains(down) || sand.Contains(down) || down.Y == bottomY))
                    {
                        unitOfSand = down;
                        continue;
                    }

                    // If the tile immediately below is blocked (by rock or sand),
                    // the unit of sand attempts to instead move diagonally one step down and to the left.
                    Coords downLeft = unitOfSand + Coords.Direction["down-left"];
                    if (!(rock.Contains(downLeft) || sand.Contains(downLeft) || down.Y == bottomY))
                    {
                        unitOfSand = downLeft;
                        continue;
                    }

                    // If that tile is blocked, the unit of sand attempts to instead move diagonally one step down and to the right.
                    Coords downRight = unitOfSand + Coords.Direction["down-right"];
                    if (!(rock.Contains(downRight) || sand.Contains(downRight) || down.Y == bottomY))
                    {
                        unitOfSand = downRight;
                        continue;
                    }

                    // If all three possible destinations are blocked, the unit of sand comes to rest and no longer moves.
                    sand.Add(unitOfSand);
                    break;
                }
            }

            // How many units of sand come to rest?
            int output = sand.Count;

            Console.WriteLine("Solution: {0}.", output);
        }

        // Each path appears as a single line of text in your scan.
        private static List<Coords> ParseRockStructure(string input)
        {
            List<Coords> endsOfLines = input
                .Split(" -> ")
                .Select(coordsInput => new Coords(coordsInput))
                .ToList();

            return endsOfLines
                .Skip(1)
                .Zip(endsOfLines)
                .SelectMany(pairOfCoords => ConstructLineOfRocks(pairOfCoords.First, pairOfCoords.Second))
                .Distinct()
                .ToList();
        }

        // After the first point of each path, each point indicates the end of a straight horizontal or vertical line to be drawn from the previous point.
        private static List<Coords> ConstructLineOfRocks(Coords lhs, Coords rhs)
        {
            if (lhs.X == rhs.X)
            {
                int minY = Math.Min(lhs.Y, rhs.Y);
                int maxY = Math.Max(lhs.Y, rhs.Y);
                return Enumerable
                    .Range(minY, maxY - minY + 1)
                    .Select(y => new Coords(lhs.X, y))
                    .ToList();
            }
            else
            {
                int minX = Math.Min(lhs.X, rhs.X);
                int maxX = Math.Max(lhs.X, rhs.X);
                return Enumerable
                    .Range(minX, maxX - minX + 1)
                    .Select(x => new Coords(x, lhs.Y))
                    .ToList();
            }
        }
    }
}
