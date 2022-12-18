using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day18
{
    public class Day18A : IDay
    {
        public void Run()
        {
            // The cooling rate should be based on the surface area of the lava droplets, so you take a quick scan of a droplet as it flies past you (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day18\Day18.txt").ToList();

            // Because of how quickly the lava is moving, the scan isn't very good; its resolution is quite low and, as a result,
            // it approximates the shape of the lava droplet with 1x1x1 cubes on a 3D grid, each given as its x,y,z position.
            HashSet<Coords> cubes = input
                .Select(entry => new Coords(entry))
                .ToHashSet();

            // What is the surface area of your scanned lava droplet?
            int output = cubes
                .Select(cube => CountUnconnectedSides(cube, cubes))
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        // To approximate the surface area, count the number of sides of each cube that are not immediately connected to another cube. 
        private static int CountUnconnectedSides(Coords cube, HashSet<Coords> cubes) => Coords.Direction
            .Select(direction => cube + direction)
            .Count(cube => !cubes.Contains(cube));
    }
}
