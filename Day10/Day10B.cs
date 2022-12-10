using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    public class Day10B : IDay
    {
        public void Run()
        {
            // The CPU uses these instructions in a program (your puzzle input) to, somehow, tell the screen what to draw.
            List<string> input = File.ReadAllLines(@"..\..\..\Day10\Day10.txt").ToList();

            // The CPU has a single register, X, which starts with the value 1.
            List<int> x = new() { 1 };

            int commandIndex = 0;
            int cyclesLeftForCommand = 0;
            int pixelBeingDrawn = 0;

            string commandName = "";
            int v = 0;

            Console.WriteLine("Solution:");

            // The clock circuit ticks at a constant rate; each tick is called a cycle.
            foreach (int cycle in Enumerable.Range(1, 240))
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

                // It seems like the X register controls the horizontal position of a sprite.
                // Specifically, the sprite is 3 pixels wide, and the X register sets the horizontal position of the middle of that sprite. 
                List<int> spritePixels = new() { x.Last() - 1, x.Last(), x.Last() + 1 };

                // If the sprite is positioned such that one of its three pixels is the pixel currently being drawn,
                // the screen produces a lit pixel (#); otherwise, the screen leaves the pixel dark (.).
                Console.Write(spritePixels.Any(pixel => pixel == pixelBeingDrawn) ? "#" : ".");

                // By carefully timing the CPU instructions and the CRT drawing operations,
                // you should be able to determine whether the sprite is visible the instant each pixel is drawn.
                ++pixelBeingDrawn;
                --cyclesLeftForCommand;

                // You count the pixels on the CRT: 40 wide and 6 high.
                if (pixelBeingDrawn == 40)
                {
                    Console.WriteLine();
                    pixelBeingDrawn = 0;
                }

                int newX = x.Last();
                if (cyclesLeftForCommand == 0 && commandName == "addx")
                { // After two cycles, the X register is increased by the value V. (V can be negative.)
                    newX += v;
                }
                x.Add(newX);
            }
        }
    }
}
