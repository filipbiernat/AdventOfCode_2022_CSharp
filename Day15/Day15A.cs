using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day15
{
    public class Day15A : IDay
    {
        public void Run()
        {
            // It doesn't take long for the sensors to report back their positions and closest beacons (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day15\Day15.txt").ToList();

            // Each sensor knows its own position and can determine the position of a beacon precisely.
            Dictionary<Coords, Coords> sensorsAndBeacons = input
                .Select(ParseSensorAndBeacon)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // For now, keep things simple by counting the positions where a beacon cannot possibly be along just a single row.
            int yOfTheAnalyzedRow = 2000000;

            // None of the detected beacons seem to be producing the distress signal,
            // so you'll need to work out where the distress beacon is by working out where it isn't.
            List<Range> ranges = sensorsAndBeacons
                .Select(sensorAndBeacon => FindRangeThatCannotContainBeacon(sensorAndBeacon.Key, sensorAndBeacon.Value, yOfTheAnalyzedRow))
                .Where(range => range.IsValid())
                .ToList();
            ranges.Sort((lhs, rhs) => lhs.Min - rhs.Min);

            // Consult the report from the sensors you just deployed.
            for (int rangeIndex = 0; rangeIndex < ranges.Count - 1; ++rangeIndex)
            {
                bool wasMerged = ranges[rangeIndex].MergeWith(ranges[rangeIndex + 1]);
                if (wasMerged)
                {
                    ranges.RemoveAt(rangeIndex + 1);
                    --rangeIndex;
                }
            }

            int positionsThatCannotContainABeacon = ranges
                .Select(range => range.GetLength())
                .Sum();

            int sensorsAndBeaconsInTheDetectedRanges = sensorsAndBeacons
                .Keys
                .Concat(sensorsAndBeacons.Values)
                .Where(coords => coords.Y == yOfTheAnalyzedRow)
                .Distinct()
                .SelectMany(coords => ranges
                    .Select(range => range.IsIn(coords.X)))
                .Count();

            // In the row where y=2000000, how many positions cannot contain a beacon?
            int output = positionsThatCannotContainABeacon - sensorsAndBeaconsInTheDetectedRanges;

            Console.WriteLine("Solution: {0}.", output);
        }

        // Sensors and beacons always exist at integer coordinates.
        private static KeyValuePair<Coords, Coords> ParseSensorAndBeacon(string input)
        {
            string[] splitInput = input.Split(new string[] { "=", ",", ":" }, StringSplitOptions.None);
            Coords sensor = new(int.Parse(splitInput[1]), int.Parse(splitInput[3]));
            Coords beacon = new(int.Parse(splitInput[5]), int.Parse(splitInput[7]));
            return new(sensor, beacon);
        }

        // Sensors can only lock on to the one beacon closest to the sensor as measured by the Manhattan distance.
        private static Range FindRangeThatCannotContainBeacon(Coords sensor, Coords beacon, int yOfTheAnalyzedRow)
        {
            int distance = Coords.ManhattanDistance(sensor, beacon);
            int xMin = sensor.X - distance + Math.Abs(sensor.Y - yOfTheAnalyzedRow);
            int xMax = sensor.X + distance - Math.Abs(sensor.Y - yOfTheAnalyzedRow);
            return new Range(xMin, xMax);
        }
    }
}
