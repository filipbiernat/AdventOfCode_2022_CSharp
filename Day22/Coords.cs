using System.Data.Common;

namespace AdventOfCode2022.Day22
{
    public class Coords
    {
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

        public void TakeAStep(Direction facingDirection)
        {
            switch (facingDirection)
            {
                case Direction.Left: --Column; break;
                case Direction.Right: ++Column; break;
                case Direction.Up: --Row; break;
                default: ++Row; break;
            }
        }

        public bool IsOutOfBounds(int max) => Row < 0 || Column < 0 || Row >= max || Column >= max;
        public bool IsInBounds(int max) => !IsOutOfBounds(max);
        public void RotateClockwise(int max) => (Row, Column) = (Column, max - Row - 1);

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.Row + rhs.Row, lhs.Column + rhs.Column);
        public static Coords operator -(Coords lhs, Coords rhs) => new(lhs.Row - rhs.Row, lhs.Column - rhs.Column);
        public static Coords operator %(Coords lhs, Coords rhs) => new(lhs.Row % rhs.Row, lhs.Column % rhs.Column);
        public static Coords operator *(Coords lhs, int rhs) => new(lhs.Row * rhs, lhs.Column * rhs);

        public override int GetHashCode() => (Row.GetHashCode() << 2) ^ Column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (Row == ((Coords)obj).Row) &&
            (Column == ((Coords)obj).Column);
    }
}
