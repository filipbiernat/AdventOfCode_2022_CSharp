namespace AdventOfCode2022.Day15
{
    public class Coords
    {
        public int X, Y;

        public Coords(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static int ManhattanDistance(Coords lhs, Coords rhs) => Math.Abs(lhs.X - rhs.X) + Math.Abs(lhs.Y - rhs.Y);
    }
}
