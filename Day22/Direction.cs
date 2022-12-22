namespace AdventOfCode2022.Day22
{
    // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^). 
    public enum Direction { Right = 0, Down = 1, Left = 2, Up = 3 };

    static class DirectionMethods
    {
        public static int GetValue(this Direction direction) => (int)direction;
        public static Direction GetRight(this Direction direction) => direction.RotateClockwiseNTimes(1);
        public static Direction GetLeft(this Direction direction) => direction.RotateClockwiseNTimes(3);
        private static Direction RotateClockwiseNTimes(this Direction direction, int n) => (Direction)(((int)direction + n) % 4);
    }
}
