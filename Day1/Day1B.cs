using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day1
{
    public class Day1B : IDay
    {
        public void Run()
        {
            // One important consideration is food - in particular, the number of Calories each Elf is carrying (your puzzle input).
            string[] input = File.ReadAllText(@"..\..\..\Day1\Day1.txt").Split("\r\n\r\n");

            // Find the Elf carrying the most Calories. How many total Calories is that Elf carrying?
            int output = input
                .Select(calloriesCarriedByOneElf => calloriesCarriedByOneElf
                    .Split("\r\n")
                    .Select(int.Parse)
                    .Sum())
                .OrderByDescending(sumOfCalloriesCarriedByOneElf => sumOfCalloriesCarriedByOneElf)
                .Take(3)
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
