using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day2
{
    public class Day2B : IDay
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
            // The first column is what your opponent is going to play.
            Move opponentsMove = DecodeMove(twoColumnsOfTheGuideForOneRound[0]);
            // The second column says how the round needs to end.
            Outcome outcome = DecodeOutcome(twoColumnsOfTheGuideForOneRound[1]);
            Move playersMove = FindPlayersMove(opponentsMove, outcome);
            return new RoundOfRockPaperScissors(playersMove, opponentsMove);
        }

        // A for Rock, B for Paper, and C for Scissors
        private static Move DecodeMove(string move)
        {
            if (move == "A") return Move.Rock;
            if (move == "B") return Move.Paper;
            return Move.Scissors;
        }

        // X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win.
        private static Outcome DecodeOutcome(string outcome)
        {
            if (outcome == "X") return Outcome.Loss;
            if (outcome == "Y") return Outcome.Draw;
            return Outcome.Win;
        }

        private static Move FindPlayersMove(Move opponentsMove, Outcome outcome)
        {
            // Rock defeats Scissors.
            if (outcome == Outcome.Loss && opponentsMove == Move.Rock) return Move.Scissors;
            if (outcome == Outcome.Win && opponentsMove == Move.Scissors) return Move.Rock;
            // Scissors defeats Paper.
            if (outcome == Outcome.Loss && opponentsMove == Move.Scissors) return Move.Paper;
            if (outcome == Outcome.Win && opponentsMove == Move.Paper) return Move.Scissors;
            // Paper defeats Rock.
            if (outcome == Outcome.Loss && opponentsMove == Move.Paper) return Move.Rock;
            if (outcome == Outcome.Win && opponentsMove == Move.Rock) return Move.Paper;
            return opponentsMove;
        }
    }
}
