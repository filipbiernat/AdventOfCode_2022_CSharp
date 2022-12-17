using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day16
{
    public class Day16B : IDay
    {
        private static readonly Dictionary<string, int> FlowRates = new();
        private static readonly Dictionary<string, List<string>> NextValves = new();
        private static readonly Stack<StateToExplore> StatesToExplore = new();

        public void Run()
        {
            // You scan the cave for other options and discover a network of pipes and pressure-release valves.
            // You aren't sure how such a system got into a volcano, but you don't have time to complain; your device produces a report (your puzzle input)
            // of each valve's flow rate if it were opened (in pressure per minute) and the tunnels you could use to move between the valves.
            List<string> input = File.ReadAllLines(@"..\..\..\Day16\Day16.txt").ToList();
            input.ForEach(ParseValveInput);

            // There's even a valve in the room you and the elephants are currently standing in labeled AA.
            StatesToExplore.Push(new StateToExplore(
                time: 1,
                myCurrentValve: "AA",
                elephantsCurrentValve: "AA",
                pressureReleased: 0,
                openedValves: new()));

            // What is the most pressure you could release?
            Dictionary<TimeAndLocation, int> pressureReleased = new();

            while (StatesToExplore.Any())
            {
                StateToExplore currentStateToExplore = StatesToExplore.Pop();
                TimeAndLocation currentTimeAndLocation = currentStateToExplore.TimeAndLocation;

                // You need to release as much pressure as possible, so you'll need to be methodical.
                if (!pressureReleased.ContainsKey(currentTimeAndLocation) || pressureReleased[currentTimeAndLocation] < currentStateToExplore.PressureReleased) // Use cache.
                {
                    pressureReleased[currentTimeAndLocation] = currentStateToExplore.PressureReleased;

                    // It would take you 4 minutes to teach an elephant how to open the right valves in the right order, leaving you with only 26 minutes to actually execute your plan.
                    if (currentTimeAndLocation.Time < 26)
                    {
                        // You estimate it will take you one minute to open a single valve and one minute to follow any tunnel from one valve to another.
                        if (CanOpenValve(currentTimeAndLocation.MyCurrentValve, currentStateToExplore.OpenedValves)) // Open my current valve.
                        {
                            currentStateToExplore.OpenedValves.Add(currentTimeAndLocation.MyCurrentValve);
                            ConsiderNextOptionsForTheElephant(currentStateToExplore, currentTimeAndLocation.MyCurrentValve);
                            currentStateToExplore.OpenedValves.Remove(currentTimeAndLocation.MyCurrentValve);
                        }
                        foreach (string myNextValve in NextValves[currentTimeAndLocation.MyCurrentValve]) // Consider my moves to the next valves.
                        {
                            ConsiderNextOptionsForTheElephant(currentStateToExplore, myNextValve);
                        }
                    }
                }
            }

            // With you and an elephant working together for 26 minutes, what is the most pressure you could release?
            int output = pressureReleased.Values.Max();

            Console.WriteLine("Solution: {0}.", output);
        }

        private static void ConsiderNextOptionsForTheElephant(StateToExplore currentStateToExplore, string myNextValve)
        {
            TimeAndLocation currentTimeAndLocation = currentStateToExplore.TimeAndLocation;

            if (CanOpenValve(currentTimeAndLocation.ElephantsCurrentValve, currentStateToExplore.OpenedValves)) // The elephant opens its current valve.
            {
                currentStateToExplore.OpenedValves.Add(currentTimeAndLocation.ElephantsCurrentValve);
                StatesToExplore.Push(new StateToExplore(
                    currentTimeAndLocation.Time + 1,
                    myNextValve,
                    currentTimeAndLocation.ElephantsCurrentValve,
                    currentStateToExplore.PressureReleased + CalculatePressureForOpenedValves(currentStateToExplore.OpenedValves),
                    currentStateToExplore.OpenedValves));
                currentStateToExplore.OpenedValves.Remove(currentTimeAndLocation.ElephantsCurrentValve);
            }

            int newPressureReleased = currentStateToExplore.PressureReleased + CalculatePressureForOpenedValves(currentStateToExplore.OpenedValves);
            foreach (string elephantsNextValve in NextValves[currentTimeAndLocation.ElephantsCurrentValve]) // Consider the elephant's moves to the next valves.
            {
                StatesToExplore.Push(new StateToExplore(
                    currentTimeAndLocation.Time + 1,
                    myNextValve,
                    elephantsNextValve,
                    newPressureReleased,
                    currentStateToExplore.OpenedValves));
            }
        }

        private static bool CanOpenValve(string valveToOpen, List<string> openedValves) => FlowRates[valveToOpen] > 0 && !openedValves.Contains(valveToOpen);
        private static int CalculatePressureForOpenedValves(List<string> openedValves) => openedValves
            .Where(openedValve => FlowRates.ContainsKey(openedValve))
            .Select(openedValve => FlowRates[openedValve])
            .Sum();

        private static void ParseValveInput(string input)
        {
            string[] splitInput = input.Split(new string[] { "=", ", ", ":", "; ", " " }, StringSplitOptions.TrimEntries);
            FlowRates[splitInput[1]] = int.Parse(splitInput[5]);
            NextValves[splitInput[1]] = splitInput[10..].ToList();
        }

        private class TimeAndLocation
        {
            public int Time;
            public string MyCurrentValve;
            public string ElephantsCurrentValve;

            public TimeAndLocation(int time, string myCurrentValve, string elephantsCurrentValve)
            {
                Time = time;
                MyCurrentValve = myCurrentValve;
                ElephantsCurrentValve = elephantsCurrentValve;
            }

            public override int GetHashCode() => string.Format("{0}-{1}-{2}", Time, MyCurrentValve, ElephantsCurrentValve).GetHashCode();
            public override bool Equals(object? obj) =>
                (obj != null) &&
                (Time == ((TimeAndLocation)obj).Time) &&
                (MyCurrentValve == ((TimeAndLocation)obj).MyCurrentValve) &&
                (ElephantsCurrentValve == ((TimeAndLocation)obj).ElephantsCurrentValve);
        }

        private class StateToExplore
        {
            public TimeAndLocation TimeAndLocation;
            public int PressureReleased;
            public List<string> OpenedValves;

            public StateToExplore(int time, string myCurrentValve, string elephantsCurrentValve, int pressureReleased, List<string> openedValves)
            {
                TimeAndLocation = new(time, myCurrentValve, elephantsCurrentValve);
                PressureReleased = pressureReleased;
                OpenedValves = new List<string>(openedValves);
            }
        }
    }
}
