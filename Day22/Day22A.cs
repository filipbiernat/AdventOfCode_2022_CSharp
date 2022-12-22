using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day22
{
    public class Day22A : IDay
    {
        public void Run()
        {
            // As you walk, the monkeys explain that the grove is protected by a force field.
            // To pass through the force field, you have to enter a password; doing so involves tracing a specific path on a strangely-shaped board.
            // The monkeys give you notes that they took when they last saw the password entered (your puzzle input).
            string[] input = File.ReadAllText(@"..\..\..\Day22\Day22.txt").Split("\r\n\r\n");

            // The first half of the monkeys' notes is a map of the board.
            // It is comprised of a set of open tiles (on which you can move, drawn .) and solid walls (tiles which you cannot enter, drawn #).
            HashSet<Coords> openTiles = input
                .First()
                .Split("\r\n")
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, char>(
                        new Coords(rowIndex, colIndex), elem)))
                .Where(pair => pair.Value == '.')
                .Select(pair => pair.Key)
                .ToHashSet();

            // The second half is a description of the path you must follow. It consists of alternating numbers and letters.
            List<string> path = Regex
                .Matches(input.Last(), @"L|R|\d+")
                .Select(match => match.Value)
                .ToList();

            // You begin the path in the leftmost open tile of the top row of tiles.
            // Initially, you are facing to the right (from the perspective of how the map is drawn).
            CoordsAndDirection currentCoordsAndDirection = new(new Coords(1, 51), Direction.Right);

            // Follow the path given in the monkeys' notes.
            path.ForEach(pathElement =>
            {
                if (pathElement == "L")
                {
                    // A letter indicates whether to turn 90 degrees clockwise (R) or counterclockwise (L). Turning happens in-place; it does not change your current tile.
                    currentCoordsAndDirection.TurnLeft();
                }
                else if (pathElement == "R")
                {
                    currentCoordsAndDirection.TurnRight();
                }
                else
                {
                    // A number indicates the number of tiles to move in the direction you are facing.
                    for (int stepIndex = 0; stepIndex < int.Parse(pathElement); ++stepIndex)
                    {
                        CoordsAndDirection nextCoordsAndDirection = CoordsAndDirection.NextCoordsAndDirection(currentCoordsAndDirection, NextRegionMap);
                        if (!openTiles.Contains(nextCoordsAndDirection.Coords))
                        {
                            // If you run into a wall, you stop moving forward and continue with the next instruction.
                            break;
                        }
                        currentCoordsAndDirection = nextCoordsAndDirection;
                    }
                }
            });

            // What is the final password?
            int output = currentCoordsAndDirection.GetPassword();

            Console.WriteLine("Solution: {0}.", output);
        }

        // Trick: We can represent our grid as 6 independent neighbouring regions:
        //  12
        //  3
        // 45
        // 6
        // If we step out of the region, we need to know what region we will enter.
        // That region depends on the facing direction.
        private static readonly Dictionary<int, Dictionary<Direction, int>> NextRegionMap = new()
        {
            { 1, new () { { Direction.Right, 2 }, { Direction.Down, 3 }, { Direction.Left, 2 }, { Direction.Up, 5 } } },
            { 2, new () { { Direction.Right, 1 }, { Direction.Down, 2 }, { Direction.Left, 1 }, { Direction.Up, 2 } } },
            { 3, new () { { Direction.Right, 3 }, { Direction.Down, 5 }, { Direction.Left, 3 }, { Direction.Up, 1 } } },
            { 4, new () { { Direction.Right, 5 }, { Direction.Down, 6 }, { Direction.Left, 5 }, { Direction.Up, 6 } } },
            { 5, new () { { Direction.Right, 4 }, { Direction.Down, 1 }, { Direction.Left, 4 }, { Direction.Up, 3 } } },
            { 6, new () { { Direction.Right, 6 }, { Direction.Down, 4 }, { Direction.Left, 6 }, { Direction.Up, 4 } } },
        };
    }
}
