﻿namespace AdventOfCode2022
{
    class AdventOfCode2022
    {
        public static void Main()
        {
            Console.WriteLine("Advent Of Code 2022");
            Execute(new Day1.Day1A(), new Day1.Day1B());
            Execute(new Day2.Day2A(), new Day2.Day2B());
            Execute(new Day3.Day3A(), new Day3.Day3B());
            Execute(new Day4.Day4A(), new Day4.Day4B());
        }

        private static void Execute(params IDay[] days)
        {
            foreach (IDay day in days)
            {
                Execute(day);
            }
        }

        private static void Execute(IDay day)
        {
            Console.WriteLine();
            Console.WriteLine("Running {0}:", day.GetType().Name);

            System.Diagnostics.Stopwatch stopWatch = new();
            stopWatch.Start();
            day.Run();
            stopWatch.Stop();

            List<string> runTime = new();
            AppendTime(runTime, stopWatch.Elapsed.Hours, "h");
            AppendTime(runTime, stopWatch.Elapsed.Minutes, "min");
            AppendTime(runTime, stopWatch.Elapsed.Seconds, "sec");
            AppendTime(runTime, stopWatch.Elapsed.Milliseconds, "msec");
            Console.WriteLine("Run time: {0}.", string.Join(", ", runTime));
        }

        private static void AppendTime(List<string> list, int time, string unit)
        {
            if (time > 0)
            {
                list.Add(string.Format($"{time} {unit}"));
            }
        }
    }
}