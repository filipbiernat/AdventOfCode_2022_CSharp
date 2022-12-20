using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day20
{
    public class Day20B : IDay
    {
        public void Run()
        {
            // Fortunately, your handheld device has a file (your puzzle input) that contains the grove's coordinates!
            // Unfortunately, the file is encrypted - just in case the device were to fall into the wrong hands.
            List<string> input = File.ReadAllLines(@"..\..\..\Day20\Day20.txt").ToList();

            // The encrypted file is a list of numbers.
            List<long> encryptedFile = input.Select(long.Parse).ToList();
            List<int> positionsOfTheNumbers = Enumerable.Range(0, encryptedFile.Count).ToList();

            // First, you need to apply the decryption key, 811589153.
            long decryptionKey = 811589153;
            // Multiply each number by the decryption key before you begin; this will produce the actual list of numbers to mix.
            encryptedFile = encryptedFile.Select(number => number * decryptionKey).ToList();

            // Second, you need to mix the list of numbers ten times.
            // The order in which the numbers are mixed does not change during mixing; the numbers are still moved in the order they appeared in the original, pre-mixed list.
            for (int mixIndex = 0; mixIndex < 10; ++mixIndex)
            {
                // Numbers moving around during the mixing process do not change the order in which the numbers are moved.
                for (int initialPositionOfTheNumberToMove = 0; initialPositionOfTheNumberToMove < encryptedFile.Count; ++initialPositionOfTheNumberToMove)
                {
                    // To mix the file, move each number forward or backward in the file a number of positions equal to the value of the number being moved.
                    int currentPositionOfTheNumberToMove = positionsOfTheNumbers.IndexOf(initialPositionOfTheNumberToMove);
                    long numberToMove = encryptedFile[initialPositionOfTheNumberToMove];
                    long stepsToMoveTheNumber = Math.Abs(numberToMove) % (encryptedFile.Count - 1);
                    int singleStepSize = numberToMove >= 0 ? 1 : -1;

                    for (int stepIndex = 0; stepIndex < stepsToMoveTheNumber; ++stepIndex)
                    {
                        // The list is circular, so moving a number off one end of the list wraps back around to the other end as if the ends were connected.
                        int nextPositionOfTheNumberToMove = (currentPositionOfTheNumberToMove + singleStepSize + encryptedFile.Count) % encryptedFile.Count;

                        int valueToSwap = positionsOfTheNumbers[nextPositionOfTheNumberToMove];
                        positionsOfTheNumbers[nextPositionOfTheNumberToMove] = positionsOfTheNumbers[currentPositionOfTheNumberToMove];
                        positionsOfTheNumbers[currentPositionOfTheNumberToMove] = valueToSwap;

                        currentPositionOfTheNumberToMove = nextPositionOfTheNumberToMove;
                    }
                }
            }

            // Apply the decryption key and mix your encrypted file ten times.
            List<long> decryptedFile = positionsOfTheNumbers
                .Select(positionOfTheNumber => encryptedFile[positionOfTheNumber])
                .ToList();

            // Then, the grove coordinates can be found by looking at the 1000th, 2000th, and 3000th numbers after the value 0, wrapping around the list as necessary. 
            int indexOfTheValue0 = decryptedFile.IndexOf(0);
            List<int> positionsOfTheGroveCoordinates = new List<int>() { 1000, 2000, 3000 }
                .Select(position => position + indexOfTheValue0)
                .Select(position => position % decryptedFile.Count)
                .ToList();

            // What is the sum of the three numbers that form the grove coordinates?
            long output = positionsOfTheGroveCoordinates
                .Select(position => decryptedFile[position])
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
