using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    public class Day10A : IDay
    {
        public void Run()
        {
            // The CPU uses these instructions in a program (your puzzle input) to, somehow, tell the screen what to draw.
            List<string> input = File.ReadAllLines(@"..\..\..\Day10\Day10.txt").ToList();

            // The CPU has a single register, X, which starts with the value 1.
            List<int> x = new() { 1 };

            int commandIndex = 0;
            int cyclesLeftForCommand = 0;

            string commandName = "";
            int v = 0;

            // The clock circuit ticks at a constant rate; each tick is called a cycle.
            foreach (int cycle in Enumerable.Range(1, 220))
            {
                if (cyclesLeftForCommand == 0)
                {
                    string[] splitInstruction = input[commandIndex].Split(' ');
                    ++commandIndex;
                    commandName = splitInstruction.First();

                    if (commandName == "noop")
                    { // noop takes one cycle to complete. It has no other effect.
                        cyclesLeftForCommand = 1;
                    }
                    else if (commandName == "addx")
                    { // addx V takes two cycles to complete.
                        cyclesLeftForCommand = 2;
                        v = int.Parse(splitInstruction.Last());
                    }
                }

                --cyclesLeftForCommand;

                int newX = x.Last();
                if (cyclesLeftForCommand == 0 && commandName == "addx")
                { // After two cycles, the X register is increased by the value V. (V can be negative.)
                    newX += v;
                }
                x.Add(newX);
            }

            // Find the signal strength during the 20th, 60th, 100th, 140th, 180th, and 220th cycles. What is the sum of these six signal strengths?
            int output = new List<int>() { 20, 60, 100, 140, 180, 220 }
                .Select(cycle => FindTheSignalStrength(x, cycle))
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        // Signal strength (the cycle number multiplied by the value of the X register).
        private int FindTheSignalStrength(List<int> x, int cycle) => cycle * x[cycle - 1];
    }
}
