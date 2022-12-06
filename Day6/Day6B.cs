using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day6
{
    public class Day6B : IDay
    {
        public void Run()
        {
            // To fix the communication system, you need to add a subroutine to the device that detects a start-of-packet marker in the datastream.
            // The device will send your subroutine a datastream buffer (your puzzle input).
            string input = File.ReadAllLines(@"..\..\..\Day6\Day6.txt").First();

            // A start-of-message marker is just like a start-of-packet marker, except it consists of 14 distinct characters rather than 4.
            int numOfDifferentCharacters = 14;

            // How many characters need to be processed before the first start-of-message marker is detected?
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
