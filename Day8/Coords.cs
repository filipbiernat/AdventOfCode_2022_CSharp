namespace AdventOfCode2022.Day8
{
    public class Coords
    {
        public int Row;
        public int Column;

        public Coords(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override int GetHashCode() => (Row.GetHashCode() << 2) ^ Column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Row == ((Coords)obj).Row) &&
            (Column == ((Coords)obj).Column);
    }
}
