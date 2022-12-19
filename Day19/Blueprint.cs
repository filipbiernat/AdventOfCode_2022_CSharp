using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day19
{
    public class Blueprint
    {
        private readonly int Id;
        // Collecting ore requires ore-collecting robots with big drills.
        private readonly Resources CostOfOreRobot;
        // In order to harvest the clay, you'll need special-purpose clay-collecting robots.
        private readonly Resources CostOfClayRobot;
        // To collect the obsidian from the bottom of the pond, you'll need waterproof obsidian-collecting robots.
        private readonly Resources CostOfObsidianRobot;
        // Perhaps you could use the obsidian to create some geode-cracking robots and break them open?
        private readonly Resources CostOfGeodeRobot;

        public Blueprint(string input)
        {
            List<int> parsedInput = Regex
                .Matches(input, @"\d+")
                .Select(match => match.Value)
                .Select(int.Parse)
                .ToList();

            Id = parsedInput[0];

            CostOfOreRobot = new Resources(ore: parsedInput[1]);
            CostOfClayRobot = new Resources(ore: parsedInput[2]);
            CostOfObsidianRobot = new Resources(ore: parsedInput[3], clay: parsedInput[4]);
            CostOfGeodeRobot = new Resources(ore: parsedInput[5], obsidian: parsedInput[6]);
        }

        // Determine the quality level of each blueprint by multiplying that blueprint's ID number
        // with the largest number of geodes that can be opened in 24 minutes using that blueprint. 
        public int DetermineQualityLevel(int minutes) => Id * FindMaxNumOfGeodes(minutes);

        // Fortunately, you have exactly one ore-collecting robot in your pack that you can use to kickstart the whole operation.
        public int FindMaxNumOfGeodes(int minutes)
        {
            Resources collectingRobots = new(ore: 1);
            State stateAtTheBeginning = new(minutes, resources: new(), collectingRobots);
            return FindMaxNumOfGeodes(stateAtTheBeginning);
        }

        // Each robot can collect 1 of its resource type per minute.
        // It also takes one minute for the robot factory (also conveniently from your pack) to construct any type of robot,
        // although it consumes the necessary resources available when construction begins.
        private int FindMaxNumOfGeodes(State currentState) => currentState.IsTimeOver()
            ? currentState.GetNumOfGeodes()
            : DiscoverNextSteps(currentState)
                .Select(nextState => nextState.CollectResources(currentState))
                .Select(nextState => FindMaxNumOfGeodes(nextState))
                .Max();

        private List<State> DiscoverNextSteps(State state)
        {
            if (state.CanStartBuildingRobot(CostOfGeodeRobot))
            {
                return new List<State>() { state.BuildRobot("Geode", CostOfGeodeRobot) };
            }

            List<State> nextSteps = new() { state };
            if (state.CanStartBuildingRobot(CostOfObsidianRobot))
            {
                nextSteps.Add(state.BuildRobot("Obsidian", CostOfObsidianRobot));
            }
            if (state.CanStartBuildingRobot(CostOfClayRobot))
            {
                nextSteps.Add(state.BuildRobot("Clay", CostOfClayRobot));
            }
            if (state.CanStartBuildingRobot(CostOfOreRobot))
            {
                nextSteps.Add(state.BuildRobot("Ore", CostOfOreRobot));
            }
            return nextSteps;
        }
    }
}
