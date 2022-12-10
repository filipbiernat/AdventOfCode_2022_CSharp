namespace AdventOfCode2022.Day9
{
    public class Coords
    {
        public static readonly Dictionary<string, Coords> Direction = new()
        {
            { "L", new Coords(0, -1) },
            { "R", new Coords(0, 1) },
            { "U", new Coords(-1, 0) },
            { "D", new Coords(1, 0) },
        };

        public int Row;
        public int Column;

        public Coords(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.Row + rhs.Row, lhs.Column + rhs.Column);
        public static Coords operator -(Coords lhs, Coords rhs) => new(lhs.Row - rhs.Row, lhs.Column - rhs.Column);
        public override int GetHashCode() => (Row.GetHashCode() << 2) ^ Column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Row == ((Coords)obj).Row) &&
            (Column == ((Coords)obj).Column);

        public static int Distance(Coords lhs, Coords rhs) => Math.Max(Math.Abs(lhs.Row - rhs.Row), Math.Abs(lhs.Column - rhs.Column));

        public static Coords DivideAndRoundFurtherFrom0(Coords lhs, int rhs) => new(
            DivideAndRoundFartherAwayFrom0(lhs.Row, rhs),
            DivideAndRoundFartherAwayFrom0(lhs.Column, rhs));

        private static int DivideAndRoundFartherAwayFrom0(int lhs, int rhs)
        {
            double res = (double) lhs / rhs;
            return (int)(res < 0 ? Math.Floor(res) : Math.Ceiling(res));
        }
    }
}
