using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day25
{
    public class Day25A : IDay
    {
        public void Run()
        {
            // You make a list of all of the fuel requirements (your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day25\Day25.txt").ToList();

            // To heat the fuel, Bob needs to know the total amount of fuel that will be processed ahead of time so it can correctly calibrate heat output and flow rate.
            // This amount is simply the sum of the fuel requirements of all of the hot air balloons, and those fuel requirements are even listed clearly on the side of each hot air balloon's burner.
            long sumOfTheFuelRequirements = input
                .Select(FromSNAFU)
                .Sum();

            // What SNAFU number do you supply to Bob's console?
            string output = ToSNAFU(sumOfTheFuelRequirements);

            Console.WriteLine("Solution: {0}.", output);

        }

        private static long FromSNAFU(string value) => value
            .Reverse()
            .Select((digit, position) => OneDigitFromSNAFU[digit] * (long)Math.Pow(5, position))
            .Sum();

        private static string ToSNAFU(long value) => string.Join(null, DeconstructDecimalNumber(value)
            .Select(digit => OneDigitToSNAFU[digit])
            .Reverse());

        // Instead of using digits four through zero, the digits are 2, 1, 0, minus (written -), and double-minus (written =).
        // Minus is worth -1, and double-minus is worth -2."
        private static readonly Dictionary<char, long> OneDigitFromSNAFU = new()
        {
            { '2', 2 },
            { '1', 1 },
            { '0', 0 },
            { '-', -1 },
            { '=', -2 },
        };

        // You can do it the other direction, too.
        private static readonly Dictionary<long, char> OneDigitToSNAFU = new()
        {
            { 4, '-'},
            { 3, '='},
            { 2, '2'},
            { 1, '1'},
            { 0, '0'},
        };

        // SNAFU works the same way, except it uses powers of five instead of ten.
        // Starting from the right, you have a ones place, a fives place, a twenty-fives place, a one-hundred-and-twenty-fives place, and so on.
        private static IEnumerable<long> DeconstructDecimalNumber(long value)
        {
            List<long> deconstructedNumber = new();
            do
            {
                deconstructedNumber.Add(value % 5);
                value /= 5;
                if (deconstructedNumber.Last() > 2)
                { // Trick! For digits 3 and 4, increment the value as those are negative numbers in SNAFU.
                    ++value;
                }
            } while (value > 0);
            return deconstructedNumber;
        }
    }
}
