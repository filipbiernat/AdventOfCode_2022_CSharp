namespace AdventOfCode2022.Day19
{
    public class Resources
    {
        private readonly int Ore;
        private readonly int Clay;
        private readonly int Obsidian;
        private readonly int Geode;

        public Resources(int ore = 0, int clay = 0, int obsidian = 0, int geode = 0)
        {
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
            Geode = geode;
        }

        public int GetNumOfGeodes() => Geode;

        public static Resources operator+(Resources lhs, Resources rhs) => new(lhs.Ore + rhs.Ore, lhs.Clay + rhs.Clay, lhs.Obsidian + rhs.Obsidian, lhs.Geode + rhs.Geode);
        public static Resources operator-(Resources lhs, Resources rhs) => new(lhs.Ore - rhs.Ore, lhs.Clay - rhs.Clay, lhs.Obsidian - rhs.Obsidian, lhs.Geode - rhs.Geode);
        public static bool operator<=(Resources lhs, Resources rhs) => lhs.Ore <= rhs.Ore && lhs.Clay <= rhs.Clay && lhs.Obsidian <= rhs.Obsidian && lhs.Geode <= rhs.Geode;
        public static bool operator>=(Resources lhs, Resources rhs) => lhs.Ore >= rhs.Ore && lhs.Clay >= rhs.Clay && lhs.Obsidian >= rhs.Obsidian && lhs.Geode >= rhs.Geode;

        public static readonly Dictionary<string, Resources> FromString = new()
        {
            { "Ore",  new Resources(ore: 1) },
            { "Clay", new Resources(clay: 1) },
            { "Obsidian", new Resources(obsidian: 1) },
            { "Geode", new Resources(geode: 1) },
        };
    }
}
