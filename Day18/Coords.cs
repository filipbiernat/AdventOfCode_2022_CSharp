namespace AdventOfCode2022.Day18
{
    public class Coords
    {
        public static readonly List<Coords> Direction = new()
        {
            new Coords(x: -1), new Coords(x: 1),
            new Coords(y: -1), new Coords(y: 1),
            new Coords(z: -1), new Coords(z: 1),
        };

        public int X, Y, Z;

        public Coords(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coords(string coords)
        {
            List<int> splitCoords = coords
                .Split(',')
                .Select(int.Parse)
                .ToList();

            X = splitCoords[0];
            Y = splitCoords[1];
            Z = splitCoords[2];
        }

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        public static bool operator <=(Coords lhs, Coords rhs) => (lhs.X <= rhs.X) && (lhs.Y <= rhs.Y) && (lhs.Z <= rhs.Z);
        public static bool operator >=(Coords lhs, Coords rhs) => (lhs.X >= rhs.X) && (lhs.Y >= rhs.Y) && (lhs.Z >= rhs.Z);
        public override int GetHashCode() => (Z.GetHashCode() << 4) ^ (Y.GetHashCode() << 2) ^ Z.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (X == ((Coords)obj).X) &&
            (Y == ((Coords)obj).Y) &&
            (Z == ((Coords)obj).Z);
    }
}
