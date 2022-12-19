namespace AdventOfCode2022.Day19
{
    public class State
    {
        private int RemainingMinutes;
        private Resources Resources;
        private Resources CollectingRobots;

        public State(int remainingTime, Resources resources, Resources collectingRobots)
        {
            RemainingMinutes = remainingTime;
            Resources = resources;
            CollectingRobots = collectingRobots;
        }

        public State(State other)
        {
            RemainingMinutes = other.RemainingMinutes;
            Resources = other.Resources;
            CollectingRobots = other.CollectingRobots;
        }

        public bool IsTimeOver() => RemainingMinutes == 0;
        public int GetNumOfGeodes() => Resources.GetNumOfGeodes();

        // Trick: Start building the robot only if enough resources now and not enough resources before the collection of the resources.
        public bool CanStartBuildingRobot(Resources costOfTheRobot) => Resources >= costOfTheRobot && !((Resources - CollectingRobots) >= costOfTheRobot);

        public State BuildRobot(string typeOfTheRobot, Resources costOfTheRobot) => new(this)
        {
            Resources = Resources - costOfTheRobot,
            CollectingRobots = CollectingRobots + Resources.FromString[typeOfTheRobot]
        };

        public State CollectResources(State previousState)
        {
            RemainingMinutes -= 1;
            Resources += previousState.CollectingRobots;
            return this;
        }
    }
}
