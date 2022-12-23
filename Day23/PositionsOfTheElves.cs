namespace AdventOfCode2022.Day23
{
    public class PositionsOfTheElves
    {
        private readonly HashSet<Coords> Positions;
        private readonly List<Coords> OrderOfDirections;
        private bool IsSimulationInProgress;

        public PositionsOfTheElves(HashSet<Coords> positions)
        {
            Positions = positions;
            OrderOfDirections = AdjacentPositions.Keys.ToList();
            IsSimulationInProgress = true;
        }

        public HashSet<Coords> Get() => Positions;

        public int SimulateRounds(int numOfRounds = int.MaxValue)
        {
            int roundIndex = 0;
            while (roundIndex < numOfRounds && IsSimulationInProgress)
            {
                SimulateRound();
                ++roundIndex;
            }
            return roundIndex;
        }

        // The process consists of some number of rounds during which Elves alternate between considering where to move and actually moving.
        private void SimulateRound()
        {
            Dictionary<Coords, List<Coords>> elvesWhichProposeToMoveToTheTargetPositions = new();

            // During the first half of each round, each Elf considers the eight positions adjacent to themself.
            foreach (Coords positionOfTheElf in Positions)
            {
                // If no other Elves are in one of those eight positions, the Elf does not do anything during this round. 
                bool theElfDoesNotDoAnything = Directions.Values
                    .Select(direction => positionOfTheElf + direction)
                    .All(potentialPosition => !Positions.Contains(potentialPosition));

                if (!theElfDoesNotDoAnything)
                {
                    // Otherwise, the Elf looks in each of four directions in the following order and proposes moving one step in the first valid direction.
                    foreach (Coords directionTheElfIsLookingIn in OrderOfDirections)
                    {
                        // If there is no Elf in the (...) adjacent positions, the Elf proposes moving (...) one step.
                        bool canTheElfProposeThisDirection = AdjacentPositions[directionTheElfIsLookingIn]
                            .Select(adjacentPositionDirection => positionOfTheElf + adjacentPositionDirection)
                            .All(adjacentPosition => !Positions.Contains(adjacentPosition));

                        if (canTheElfProposeThisDirection)
                        {
                            Coords positionSelectedByTheElf = positionOfTheElf + directionTheElfIsLookingIn;

                            if (elvesWhichProposeToMoveToTheTargetPositions.ContainsKey(positionSelectedByTheElf))
                            {
                                elvesWhichProposeToMoveToTheTargetPositions[positionSelectedByTheElf].Add(positionOfTheElf);
                            }
                            else
                            {
                                elvesWhichProposeToMoveToTheTargetPositions[positionSelectedByTheElf] = new() { positionOfTheElf };
                            }
                            break;
                        }
                    }
                }
            }

            // After each Elf has had a chance to propose a move, the second half of the round can begin.
            // Simultaneously, each Elf moves to their proposed destination tile if they were the only Elf to propose moving to that position.
            // If two or more Elves propose moving to the same position, none of those Elves move.
            IsSimulationInProgress = false;
            elvesWhichProposeToMoveToTheTargetPositions
                .Select(pair => ((Coords targetPosition, List<Coords> elvesWhichProposeToMove))(pair.Key, pair.Value))
                .Where(pair => pair.elvesWhichProposeToMove.Count == 1)
                .ToList()
                .ForEach(pair =>
                {
                    Coords currentPosition = pair.elvesWhichProposeToMove.First();
                    Positions.Remove(currentPosition);
                    Positions.Add(pair.targetPosition);
                    IsSimulationInProgress = true;
                });

            // Finally, at the end of the round, the first direction the Elves considered is moved to the end of the list of directions.
            Coords firstDirection = OrderOfDirections.First();
            OrderOfDirections.Remove(firstDirection);
            OrderOfDirections.Add(firstDirection);
        }

        // Orthogonal directions are written N (north), S (south), W (west), and E (east), while diagonal directions are written NE, NW, SE, SW.
        private readonly static Dictionary<string, Coords> Directions = new()
        {
            { "N", new Coords(-1, 0) },
            { "NE", new Coords(-1, 1) },
            { "E", new Coords(0, 1) },
            { "SE", new Coords(1, 1) },
            { "S", new Coords(1, 0) },
            { "SW", new Coords(1, -1) },
            { "W", new Coords(0, -1) },
            { "NW", new Coords(-1, -1) }
        };

        private readonly static Dictionary<Coords, List<Coords>> AdjacentPositions = new()
        {
            // If there is no Elf in the N, NE, or NW adjacent positions, the Elf proposes moving north one step.
            { Directions["N"], new() { Directions["NW"], Directions["N"], Directions["NE"] } },
            // If there is no Elf in the S, SE, or SW adjacent positions, the Elf proposes moving south one step.
            { Directions["S"], new() { Directions["SW"], Directions["S"], Directions["SE"] } },
            // If there is no Elf in the W, NW, or SW adjacent positions, the Elf proposes moving west one step.
            { Directions["W"], new() { Directions["NW"], Directions["W"], Directions["SW"] } },
            // If there is no Elf in the E, NE, or SE adjacent positions, the Elf proposes moving east one step.
            { Directions["E"], new() { Directions["NE"], Directions["E"], Directions["SE"] } }
        };
    }
}
