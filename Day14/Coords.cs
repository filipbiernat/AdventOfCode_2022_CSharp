namespace AdventOfCode2022.Day14
{
    public class Coords
    {
        public static readonly Dictionary<string, Coords> Direction = new()
        {
            { "down", new Coords(0, 1) },
            { "down-left", new Coords(-1, 1) },
            { "down-right", new Coords(1, 1) },
        };

        // Your scan traces the path of each solid rock structure and reports the x,y coordinates that form the shape of the path, where x represents
        // distance to the right and y represents distance down. 
        public int X, Y;

        public Coords(string coords) : this(int.Parse(coords.Split(',').First()), int.Parse(coords.Split(',').Last())) { }
        public Coords(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
        public override int GetHashCode() => (X.GetHashCode() << 2) ^ Y.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (X == ((Coords)obj).X) &&
            (Y == ((Coords)obj).Y);
    }
}
