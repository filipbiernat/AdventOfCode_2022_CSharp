namespace AdventOfCode2022.Day15
{
    public class Range
    {
        public int Min, Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public bool IsValid() => Min <= Max;
        public bool IsIn(int val) => Min <= val && val <= Max;
        public int GetLength() => Max - Min + 1;

        public bool Contains(Range other) => Min <= other.Min && Max >= other.Max;
        public bool Overlaps(Range other) => Max >= other.Min;

        public bool MergeWith(Range other)
        {
            if (Contains(other))
            {
                Max = Math.Max(Max, other.Max);
                return true;
            }
            if (Overlaps(other))
            {
                Max = other.Max;
                return true;
            }
            return false;
        }
    }
}
