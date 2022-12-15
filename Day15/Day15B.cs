using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day15
{
    public class Day15B : IDay
    {
        public void Run()
        {
            // It doesn't take long for the sensors to report back their positions and closest beacons (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day15\Day15.txt").ToList();

            // Each sensor knows its own position and can determine the position of a beacon precisely.
            Dictionary<Coords, Coords> sensorsAndBeacons = input
                .Select(ParseSensorAndBeacon)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            long tuningFrequency = 0;

            // The distress beacon is not detected by any sensor, but the distress beacon must have x and y coordinates each no lower than 0 and no larger than 4000000.
            Parallel.For(0, 4000000 + 1, (yOfTheAnalyzedRow, state) =>
            {

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

                // Find the only possible position for the distress beacon. 
                if (ranges.Count > 1)
                {
                    // To isolate the distress beacon's signal, you need to determine its tuning frequency,
                    // which can be found by multiplying its x coordinate by 4000000 and then adding its y coordinate.
                    tuningFrequency = 4000000 * (long)(ranges.First().Max + 1) + yOfTheAnalyzedRow;
                    state.Break();
                }
            });

            // Find the only possible position for the distress beacon. What is its tuning frequency?
            long output = tuningFrequency;

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
