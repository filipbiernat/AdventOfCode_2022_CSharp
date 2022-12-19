using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day19
{
    public class Day19B : IDay
    {
        public void Run()
        {
            // The robot factory has many blueprints (your puzzle input) you can choose from, but once you've configured it with a blueprint, you can't change it.
            // You'll need to work out which blueprint is best.
            List<string> input = File.ReadAllLines(@"..\..\..\Day19\Day19.txt").ToList();

            // Unfortunately, one of the elephants ate most of your blueprint list! Now, only the first three blueprints in your list are intact.
            List<Blueprint> blueprints = input
                .Take(3)
                .Select(entry => new Blueprint(entry))
                .ToList();

            // While you were choosing the best blueprint, the elephants found some food on their own, so you're not in as much of a hurry;
            // you figure you probably have 32 minutes before the wind changes direction again and you'll need to get out of range of the erupting volcano.
            // Don't worry about quality levels; instead, just determine the largest number of geodes you could open using each of the first three blueprints.
            // What do you get if you multiply these numbers together?
            int output = blueprints
                .AsParallel()
                .Select(blueprint => blueprint.FindMaxNumOfGeodes(minutes: 32))
                .Aggregate((lhs, rhs) => lhs * rhs);

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
