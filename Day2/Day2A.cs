using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day2
{
    public partial class Day2A : IDay
    {
        public void Run()
        {
            // Rock Paper Scissors is a game between two players.
            // Each game contains many rounds; in each round, the players each simultaneously choose one of Rock, Paper, orScissors using a hand shape.
            // Then, a winner for that round is selected: Rock defeats Scissors, Scissors defeats Paper, and Paper defeats Rock.
            // If both players choose the same shape, the round instead ends in a draw.

            // Appreciative of your help yesterday, one Elf gives you an encrypted strategy guide (your puzzle input) that they say will be sure to help you win.
            List<string> input = File.ReadAllLines(@"..\..\..\Day2\Day2.txt").ToList();

            // Your total score is the sum of your scores for each round.
            // What would your total score be if everything goes exactly according to your strategy guide?
            int output = input
                .Select(guideForOneRound => ParseGuide(guideForOneRound))
                .Select(roundOfRockPaperScissors => roundOfRockPaperScissors.CalculateScore())
                .Sum();

            Console.WriteLine("Solution: {0}.", output);
        }

        private static RoundOfRockPaperScissors ParseGuide(string guideForOneRound)
        {
            string[] twoColumnsOfTheGuideForOneRound = guideForOneRound.Split();
            // The first column is what your opponent is going to play: A for Rock, B for Paper, and C for Scissors.
            Move opponentsMove = DecodeMove(twoColumnsOfTheGuideForOneRound[0], rock: "A", paper: "B");
            // The second column, you reason, must be what you should play in response: X for Rock, Y for Paper, and Z for Scissors.
            Move playersMove = DecodeMove(twoColumnsOfTheGuideForOneRound[1], rock: "X", paper: "Y");
            return new RoundOfRockPaperScissors(playersMove, opponentsMove);
        }

        private static Move DecodeMove(string move, string rock, string paper)
        {
            if (move == rock) return Move.Rock;
            if (move == paper) return Move.Paper;
            return Move.Scissors;
        }
    }
}
