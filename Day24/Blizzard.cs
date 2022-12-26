namespace AdventOfCode2022.Day24
{
    // As the expedition reaches a valley that must be traversed to reach the extraction site, you find that strong,
    // turbulent winds are pushing small blizzards of snow and sharp ice around the valley. 
    public class Blizzard
    {
        private readonly Coords Position;
        private readonly Coords Direction;
        private readonly Coords SizeOfTheMap;

        public Blizzard(Coords position, Coords direction, Coords sizeOfTheMap)
        {
            Position = position;
            Direction = direction;
            SizeOfTheMap = sizeOfTheMap;
        }

        public Blizzard(Blizzard other, Coords position)
        {
            Position = position;
            Direction = other.Direction;
            SizeOfTheMap = other.SizeOfTheMap;
        }

        // Due to conservation of blizzard energy, as a blizzard reaches the wall of the valley, a new blizzard forms on the opposite side of the valley moving in the same direction.
        public Blizzard Next()
        {
            Coords newPosition = Position + Direction;

            if (newPosition.Row == 0)
            {
                newPosition.Row = SizeOfTheMap.Row - 2;
            }
            else if (newPosition.Row == SizeOfTheMap.Row - 1)
            {
                newPosition.Row = 1;
            }

            if (newPosition.Column == 0)
            {
                newPosition.Column = SizeOfTheMap.Column - 2;
            }
            else if (newPosition.Column == SizeOfTheMap.Column - 1)
            {
                newPosition.Column = 1;
            }

            return new Blizzard(this, newPosition);
        }

        public Coords GetPosition() => Position;
    }
}
