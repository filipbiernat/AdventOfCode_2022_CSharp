using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day17
{
    public class Day17A : IDay
    {
        public void Run()
        {
            // The rocks don't spin, but they do get pushed around by jets of hot gas coming out of the walls themselves.
            // A quick scan reveals the effect the jets of hot gas will have on the rocks as they fall (your puzzle input).
            string input = File.ReadAllLines(@"..\..\..\Day17\Day17.txt").First();
            List<char> pushDirections = input.ToList();
            int pushIndex = 0;

            // The tunnels eventually open into a very tall, narrow chamber.
            // The tall, vertical chamber is exactly seven units wide.
            long rightWall = 7;

            // To prove to the elephants your simulation is accurate, they want to know how tall the tower will get after 2022 rocks have stopped.
            List<Coords> rockTower = new();
            long totalNumOfRocks = 2022;
            for (long rockIndex = 0; rockIndex < totalNumOfRocks; ++rockIndex)
            {
                // Each rock appears so that its left edge is two units away from the left wall and its bottom
                // edge is three units above the highest rock in the room (or the floor, if there isn't one).
                long rowOfTheHighestRock = rockTower.Any() ? rockTower.Max(coords => coords.Row) : 0;
                Coords leftBottom = new(rowOfTheHighestRock + 4, 2);

                // The rocks fall in the order shown above: first the - shape, then the + shape, and so on.
                // Once the end of the list is reached, the same order repeats: the - shape falls first, sixth, 11th, 16th, etc.
                long rockType = rockIndex % 5;
                List<Coords> rock = Direction[rockType]
                    .Select(rockElement => leftBottom + rockElement)
                    .ToList();

                for (long moveIndex = 0; ; ++moveIndex)
                {
                    // After a rock appears, it alternates between being pushed by a jet of hot gas one unit
                    // (in the direction indicated by the next symbol in the jet pattern) and then falling one unit down.
                    List<Coords> movedRock;
                    if (moveIndex % 2 == 0) // Left or right.
                    {
                        char pushDirection = pushDirections[pushIndex];
                        movedRock = rock
                            .Select(rockElement => rockElement + Coords.Direction[pushDirection])
                            .ToList();
                        pushIndex = (pushIndex + 1) % pushDirections.Count;
                    }
                    else // Down.
                    {
                        movedRock = rock
                            .Select(rockElement => rockElement + Coords.Direction['v'])
                            .ToList();

                        // If a downward movement would have caused a falling rock to move into the floor or an already-fallen rock,
                        // the falling rock stops where it is (having landed on something) and a new rock immediately begins falling.
                        if (FallsIntoFloor(movedRock) || FallsIntoAnotherStructure(movedRock, rockTower))
                        {
                            rockTower.AddRange(rock);
                            break;
                        }
                    }
                    // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
                    if (!FallsIntoWalls(movedRock, leftWall: -1, rightWall) && !FallsIntoFloor(movedRock) && !FallsIntoAnotherStructure(movedRock, rockTower))
                    {
                        rock = movedRock;
                    }
                }
            }

            // How many units tall will the tower of rocks be after 2022 rocks have stopped falling?
            long output = rockTower.Max(coords => coords.Row);

            Console.WriteLine("Solution: {0}.", output);
        }


        private static bool FallsIntoWalls(List<Coords> rock, long leftWall, long rightWall) => rock.Any(rockElement => rockElement.Column <= leftWall || rockElement.Column >= rightWall);
        private static bool FallsIntoFloor(List<Coords> rock) => rock.Any(rockElement => rockElement.Row <= 0);
        private static bool FallsIntoAnotherStructure(List<Coords> rock, List<Coords> anotherStructure) => rock.Intersect(anotherStructure).Any();


        // The five types of rocks have the following peculiar shapes, where # is rock and . is empty space.
        public static readonly Dictionary<long, List<Coords>> Direction = new()
        {
            // ####
            { 0, new () { new Coords(0, 0), new Coords(0, 1), new Coords(0, 2), new Coords(0, 3) } },
            // .#.
            // ###
            // .#.
            { 1, new () { new Coords(0, 1), new Coords(1, 0), new Coords(1, 1), new Coords(1, 2), new Coords(2, 1) } },
            // ..#
            // ..#
            // ###
            { 2, new () { new Coords(0, 0), new Coords(0, 1), new Coords(0, 2), new Coords(1, 2), new Coords(2, 2) } },
            // #
            // #
            // #
            // #
            { 3, new () { new Coords(0, 0), new Coords(1, 0), new Coords(2, 0), new Coords(3, 0) } },
            // ##
            // ##
            { 4, new () { new Coords(0, 0), new Coords(0, 1), new Coords(1, 0), new Coords(1, 1) } },
        };
    }
}
