namespace AdventOfCode2022.Day22
{
    public class CoordsAndDirection
    {
        // To be fair, the monkeys' map does have six 50x50 regions on it.
        private static readonly int SizeOfTheRegion = 50;
        // Trick: We can represent our grid as 6 independent neighbouring regions:
        //  12
        //  3
        // 45
        // 6
        private static readonly Dictionary<int, Coords> StartCoordsOfTheRegions = new()
        {
            { 1, new Coords(0, 1) * SizeOfTheRegion },
            { 2, new Coords(0, 2) * SizeOfTheRegion },
            { 3, new Coords(1, 1) * SizeOfTheRegion },
            { 4, new Coords(2, 0) * SizeOfTheRegion },
            { 5, new Coords(2, 1) * SizeOfTheRegion },
            { 6, new Coords(3, 0) * SizeOfTheRegion }
        };

        public Coords Coords;
        public Direction Direction;

        public CoordsAndDirection(Coords coords, Direction direction)
        {
            Coords = coords;
            Direction = direction;
        }

        public CoordsAndDirection(CoordsAndDirection other)
        {
            Coords = other.Coords;
            Direction = other.Direction;
        }

        public static CoordsAndDirection NextCoordsAndDirection(
            CoordsAndDirection current,
            Dictionary<int, Dictionary<Direction, int>> nextRegionMap,
            Dictionary<int, Dictionary<Direction, int>>? numOfClockwiseRotationsMap = null)
        {
            // To be fair, the monkeys' map does have six 50x50 regions on it.
            int currentRegionId = StartCoordsOfTheRegions
                .Select(pair => ((int RegionId, Coords StartCoordsOfTheRegions))(pair.Key, pair.Value))
                .Select(pair => ((int RegionId, Coords CoordsInTheRegion))(pair.RegionId, current.Coords - pair.StartCoordsOfTheRegions))
                .Where(pair => pair.CoordsInTheRegion.IsInBounds(max: SizeOfTheRegion))
                .First()
                .RegionId;
            int nextRegionId = currentRegionId;

            CoordsAndDirection next = new(current);
            Coords coordsInTheRegion = next.Coords - StartCoordsOfTheRegions[currentRegionId];
            coordsInTheRegion.TakeAStep(next.Direction);

            if (coordsInTheRegion.IsOutOfBounds(max: SizeOfTheRegion))
            {
                nextRegionId = nextRegionMap[currentRegionId][next.Direction];

                // Trick: Use modulo operation to switch to the next region.
                Coords maxCoordsOfTheRegion = new Coords(1, 1) * SizeOfTheRegion;
                coordsInTheRegion = (coordsInTheRegion + maxCoordsOfTheRegion) % maxCoordsOfTheRegion;

                if (numOfClockwiseRotationsMap != null)
                {
                    int numOfClockwiseRotations = numOfClockwiseRotationsMap[currentRegionId][next.Direction];
                    for (int rotationIndex = 0; rotationIndex < numOfClockwiseRotations; ++rotationIndex)
                    {
                        coordsInTheRegion.RotateClockwise(max: SizeOfTheRegion);
                        next.Direction = next.Direction.GetRight();
                    }
                }
            }

            next.Coords = StartCoordsOfTheRegions[nextRegionId] + coordsInTheRegion;
            return next;
        }

        public void TurnLeft() => Direction = Direction.GetLeft();
        public void TurnRight() => Direction = Direction.GetRight();

        // Rows start from 1 at the top and count downward; columns start from 1 at the left and count rightward.
        // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^).
        public int GetPassword() => 1000 * (Coords.Row + 1) + 4 * (Coords.Column + 1) + Direction.GetValue();
    }
}
