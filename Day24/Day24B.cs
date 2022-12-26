using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day24
{
    public class Day24B : IDay
    {
        public void Run()
        {
            // As the expedition reaches a valley that must be traversed to reach the extraction site, you find that strong, turbulent winds are pushing small blizzards of snow and sharp ice around the valley.
            // Fortunately, it's easy to see all of this from the entrance to the valley, so you make a map of the valley and the blizzards (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day24\Day24.txt").ToList();

            List<Map> maps = new() { new Map(input) };
            Map firstMap = maps.First();

            while (true) // Trick! Explore all the maps in advance. Soon, the maps start to form a cycle.
            {
                Map nextMap = maps.Last().Next();
                if (firstMap.IsEqual(nextMap))
                {
                    break;
                }
                maps.Add(nextMap);
            }

            CoordsAndTime coordsAndTime = new(firstMap.GetStartPosition());
            coordsAndTime = FindTheShortestPath(coordsAndTime, firstMap.GetExitPosition(), maps);

            // As the expedition reaches the far side of the valley, one of the Elves looks especially dismayed: He forgot his snacks at the entrance to the valley!
            // Since you're so good at dodging blizzards, the Elves humbly request that you go back for his snacks. 
            coordsAndTime = FindTheShortestPath(coordsAndTime, firstMap.GetStartPosition(), maps);
            coordsAndTime = FindTheShortestPath(coordsAndTime, firstMap.GetExitPosition(), maps);

            // What is the fewest number of minutes required to reach the goal, go back to the start, then reach the goal again?
            int output = coordsAndTime.GetTime();

            Console.WriteLine("Solution: {0}.", output);
        }

        private static CoordsAndTime FindTheShortestPath(CoordsAndTime initialCoordsAndTime, Coords targetCoords, List<Map> maps)
        { // Trick! A* Algorithm.
            HashSet<CoordsAndTime> explored = new();
            PriorityQueue<CoordsAndTime, int> queue = new();
            queue.Enqueue(initialCoordsAndTime, priority: 0);

            CoordsAndTime current;
            do
            {
                current = queue.Dequeue();

                // On each minute, you can move up, down, left, or right, or you can wait in place.
                // You and the blizzards act simultaneously, and you cannot share a position with a blizzard.
                current.FindNextCoordsWithNoBlizzards(maps)
                    .Where(next => !explored.Contains(next))
                    .ToList()
                    .ForEach(next =>
                    {
                        explored.Add(next);
                        int priority = next.GetTime() + next.GetCoords().ManhattanDistanceTo(targetCoords); // Trick! Estimate the remaining distance using Manhattan metric.
                        queue.Enqueue(next, priority);
                    });
            }
            while (current.GetCoords() != targetCoords);
            return current;
        }
    }
}
