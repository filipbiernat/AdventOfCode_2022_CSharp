using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day6
{
    public class Day6A : IDay
    {
        public void Run()
        {
            // To fix the communication system, you need to add a subroutine to the device that detects a start-of-packet marker in the datastream.
            // The device will send your subroutine a datastream buffer (your puzzle input).
            string input = File.ReadAllLines(@"..\..\..\Day6\Day6.txt").First();

            // Your subroutine needs to identify the first position where the four most recently received characters were all different.
            int numOfDifferentCharacters = 4;

            // How many characters need to be processed before the first start-of-packet marker is detected?
            int output = Enumerable
                .Range(numOfDifferentCharacters, input.Length)
                .Where(i => input
                    .Substring(i - numOfDifferentCharacters, numOfDifferentCharacters)
                    .Distinct()
                    .Count() == numOfDifferentCharacters)
                .First();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
