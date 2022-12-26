namespace AdventOfCode2022.Day24
{
    public class CoordsAndTime
    {
        private readonly Coords Coords;
        private readonly int Time;

        public CoordsAndTime(Coords coords, int time = 0)
        {
            Coords = coords;
            Time = time;
        }

        public List<CoordsAndTime> FindNextCoordsWithNoBlizzards(List<Map> maps) => Coords.MoveOptions
            .Select(moveOption => Coords + moveOption)
            .Where(nextCoords => maps[(Time + 1) % maps.Count].IsNoBlizzardAtPosition(nextCoords)) // Trick! The appropriate map of blizzards is selected based on the time.
            .Select(nextCoords => new CoordsAndTime(nextCoords, Time + 1))
            .ToList();

        public Coords GetCoords() => Coords;
        public int GetTime() => Time;
        public override int GetHashCode() => (Coords.GetHashCode() << 2) ^ Time.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Coords == ((CoordsAndTime)obj).Coords) &&
            (Time == ((CoordsAndTime)obj).Time);
    }
}
