namespace AdventOfCode2022.Day24
{
    public class Map
    {
        private readonly List<Blizzard> Blizzards;
        private readonly HashSet<Coords> PositionsOfBlizzards;

        private readonly Coords LowerBound;
        private readonly Coords UpperBound;

        private readonly Coords StartPosition;
        private readonly Coords ExitPosition;

        public Map(List<string> input)
        {
            Blizzards = ParseBlizzards(input);
            PositionsOfBlizzards = ExtractPositionsOfBlizzards(Blizzards);

            LowerBound = new Coords(1, 1);
            UpperBound = GetSizeOfTheMap(input) - LowerBound;

            // Your expedition begins in the only non-wall position in the top row and needs to reach the only non-wall position in the bottom row.
            // Clear ground - where there is currently no blizzard - is drawn as ..
            StartPosition = new Coords(0, input.First().IndexOf('.'));
            ExitPosition = new Coords(UpperBound.Row, input.Last().IndexOf('.'));
        }

        public Map(Map other, List<Blizzard> blizzards)
        {
            Blizzards = blizzards;
            PositionsOfBlizzards = ExtractPositionsOfBlizzards(Blizzards);

            LowerBound = other.LowerBound;
            UpperBound = other.UpperBound;

            StartPosition = other.StartPosition;
            ExitPosition = other.ExitPosition;
        }

        public Map Next() => new(this, Blizzards
            .Select(blizzard => blizzard.Next())
            .ToList());

        public bool IsNoBlizzardAtPosition(Coords position) =>
            position == StartPosition ||
            position == ExitPosition ||
            !position.IsOutOfBounds(LowerBound, UpperBound) &&
            !PositionsOfBlizzards.Contains(position);

        public bool IsEqual(Map other) => PositionsOfBlizzards.SetEquals(other.PositionsOfBlizzards);
        public Coords GetStartPosition() => StartPosition;
        public Coords GetExitPosition() => ExitPosition;

        private static Coords GetSizeOfTheMap(List<string> input) => new(input.Count, input.First().Length);
        private static HashSet<Coords> ExtractPositionsOfBlizzards(List<Blizzard> blizzards) => blizzards.Select(blizzard => blizzard.GetPosition()).ToHashSet();

        // Otherwise, blizzards are drawn with an arrow indicating their direction of motion: up (^), down (v), left (<), or right (>).
        private static List<Blizzard> ParseBlizzards(List<string> input) => input
            .SelectMany((row, rowIndex) => row
                .ToCharArray()
                .Select((elem, colIndex) => new Tuple<Coords, char>(new Coords(rowIndex, colIndex), elem)))
            .Where(pair => Coords.Directions.ContainsKey(pair.Item2))
            .Select(pair => new Blizzard(pair.Item1, Coords.Directions[pair.Item2], GetSizeOfTheMap(input)))
            .ToList();
    }
}
