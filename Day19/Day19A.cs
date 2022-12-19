using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day19
{
    public class Day19A : IDay
    {
        public void Run()
        {
            // The robot factory has many blueprints (your puzzle input) you can choose from, but once you've configured it with a blueprint, you can't change it.
            // You'll need to work out which blueprint is best.
            List<string> input = File.ReadAllLines(@"..\..\..\Day19\Day19.txt").ToList();
            List<Blueprint> blueprints = input
                .Select(entry => new Blueprint(entry))
                .ToList();

            // Determine the quality level of each blueprint using the largest number of geodes it could produce in 24 minutes.
            // What do you get if you add up the quality level of all of the blueprints in your list?
            int output = blueprints
                .AsParallel()
                .Select(blueprint => blueprint.DetermineQualityLevel(minutes: 24))
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
