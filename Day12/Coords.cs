namespace AdventOfCode2022.Day12
{
    public class Coords
    {
        public static readonly List<Coords> Direction = new()
        {
            new Coords(0, -1),
            new Coords(0, 1),
            new Coords(-1, 0),
            new Coords(1, 0),
        };

        public int Row;
        public int Column;

        public Coords(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.Row + rhs.Row, lhs.Column + rhs.Column);
        public override int GetHashCode() => (Row.GetHashCode() << 2) ^ Column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Row == ((Coords)obj).Row) &&
            (Column == ((Coords)obj).Column);
    }
}
