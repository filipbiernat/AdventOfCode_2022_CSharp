using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day20
{
    public class Day20A : IDay
    {
        public void Run()
        {
            // Fortunately, your handheld device has a file (your puzzle input) that contains the grove's coordinates!
            // Unfortunately, the file is encrypted - just in case the device were to fall into the wrong hands.
            List<string> input = File.ReadAllLines(@"..\..\..\Day20\Day20.txt").ToList();

            // The encrypted file is a list of numbers.
            List<long> encryptedFile = input.Select(long.Parse).ToList();
            List<int> positionsOfTheNumbers = Enumerable.Range(0, encryptedFile.Count).ToList();

            // Mix your encrypted file exactly once. 
            // The numbers should be moved in the order they originally appear in the encrypted file.
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

            // The main operation involved in decrypting the file is called mixing.
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
