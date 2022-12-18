using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day18
{
    public class Day18B : IDay
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

            Coords minCubeToDiscover = new(
                cubes.Select(cube => cube.X).Min() - 1,
                cubes.Select(cube => cube.Y).Min() - 1,
                cubes.Select(cube => cube.Z).Min() - 1);
            Coords maxCubeToDiscover = new(
                cubes.Select(cube => cube.X).Max() + 1,
                cubes.Select(cube => cube.Y).Max() + 1,
                cubes.Select(cube => cube.Z).Max() + 1);

            // Consider only cube sides that could be reached by the water and steam as the lava droplet tumbles into the pond. 
            HashSet<Coords> outsideCubes = new();
            HashSet<Coords> discoveredCubes = new();

            Queue<Coords> cubesToDiscover = new();
            cubesToDiscover.Enqueue(minCubeToDiscover);
            while (cubesToDiscover.Any())
            {
                Coords cubeToDiscover = cubesToDiscover.Dequeue();
                discoveredCubes.Add(cubeToDiscover);

                if (!(cubes.Contains(cubeToDiscover) || outsideCubes.Contains(cubeToDiscover)))
                {
                    outsideCubes.Add(cubeToDiscover);

                    Coords.Direction
                        .Select(direction => cubeToDiscover + direction)
                        .Where(nextCubeToDiscover => !discoveredCubes.Contains(nextCubeToDiscover))
                        .Where(nextCubeToDiscover => nextCubeToDiscover >= minCubeToDiscover)
                        .Where(nextCubeToDiscover => nextCubeToDiscover <= maxCubeToDiscover)
                        .ToList()
                        .ForEach(nextCube => cubesToDiscover.Enqueue(nextCube));
                }
            }

            // What is the exterior surface area of your scanned lava droplet?
            int output = cubes
                .Select(cube => CountOutsideSides(cube, outsideCubes))
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        // The steam will expand to reach as much as possible, completely displacing any air on the outside of the lava droplet but never expanding diagonally.
        private static int CountOutsideSides(Coords cube, HashSet<Coords> outsideCubes) => Coords.Direction
            .Select(direction => cube + direction)
            .Count(cube => outsideCubes.Contains(cube));
    }
}
