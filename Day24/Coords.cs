namespace AdventOfCode2022.Day24
{
    public class Coords
    {
        public static readonly List<Coords> MoveOptions = new()
        {
            new Coords(0, 0),
            new Coords(0, -1),
            new Coords(0, 1),
            new Coords(-1, 0),
            new Coords(1, 0),
        };

        public static readonly Dictionary<char, Coords> Directions = new()
        {
            { '<', new Coords(0, -1) },
            { '>', new Coords(0, 1) },
            { '^', new Coords(-1, 0) },
            { 'v', new Coords(1, 0) },
        };

        public int Row;
        public int Column;

        public Coords(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public Coords(Coords other)
        {
            Row = other.Row;
            Column = other.Column;
        }

        public bool IsOutOfBounds(Coords lowerBound, Coords upperBound) => Row < lowerBound.Row || Row >= upperBound.Row || Column < lowerBound.Column || Column >= upperBound.Column;
        public int ManhattanDistanceTo(Coords other) => Math.Abs(Row - other.Row) + Math.Abs(Column - other.Column);

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.Row + rhs.Row, lhs.Column + rhs.Column);
        public static Coords operator -(Coords lhs, Coords rhs) => new(lhs.Row - rhs.Row, lhs.Column - rhs.Column);
        public static bool operator ==(Coords lhs, Coords rhs) => lhs.Row == rhs.Row && lhs.Column == rhs.Column;
        public static bool operator !=(Coords lhs, Coords rhs) => lhs.Row != rhs.Row || lhs.Column != rhs.Column;

        public override int GetHashCode() => (Row.GetHashCode() << 2) ^ Column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Row == ((Coords)obj).Row) &&
            (Column == ((Coords)obj).Column);
    }
}
