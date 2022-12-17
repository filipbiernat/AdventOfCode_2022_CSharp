namespace AdventOfCode2022.Day17
{
    public class Coords
    {
        // In jet patterns, < means a push to the left, while > means a push to the right. 
        public static readonly Dictionary<char, Coords> Direction = new()
        {
            { '<', new Coords(0, -1) },
            { '>', new Coords(0, 1) },
            { 'v', new Coords(-1, 0) },
        };

        public long Row;
        public long Column;

        public Coords(long row = 0, long column = 0)
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
