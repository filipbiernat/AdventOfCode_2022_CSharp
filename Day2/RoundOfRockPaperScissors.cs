namespace AdventOfCode2022.Day2
{
    public enum Move { Rock, Paper, Scissors }
    public enum Outcome { Win, Draw, Loss }

    public class RoundOfRockPaperScissors
    {
        private readonly Move PlayersMove;
        private readonly Move OpponentsMove;

        public RoundOfRockPaperScissors(Move playersMove, Move opponentsMove)
        {
            PlayersMove = playersMove;
            OpponentsMove = opponentsMove;
        }

        // The score for a single round is the score for the shape you selected (///) plus the score for the outcome of the round (...).
        public int CalculateScore() => CalculateScoreForTheSelectedShape() + CalculateScoreForTheOutcome();

        // 1 for Rock, 2 for Paper, and 3 for Scissors.
        private int CalculateScoreForTheSelectedShape() => PlayersMove switch
        {
            Move.Rock => 1,
            Move.Paper => 2,
            _ => 3,
        };

        // 0 if you lost, 3 if the round was a draw, and 6 if you won.
        private int CalculateScoreForTheOutcome() => CalculateThePlayersOutcome() switch
        {
            Outcome.Win => 6,
            Outcome.Draw => 3,
            _ => 0,
        };

        private Outcome CalculateThePlayersOutcome()
        {
            if (PlayersMove == OpponentsMove) return Outcome.Draw;
            // Rock defeats Scissors.
            if (PlayersMove == Move.Rock && OpponentsMove == Move.Scissors) return Outcome.Win;
            // Scissors defeats Paper.
            if (PlayersMove == Move.Scissors && OpponentsMove == Move.Paper) return Outcome.Win;
            // Paper defeats Rock.
            if (PlayersMove == Move.Paper && OpponentsMove == Move.Rock) return Outcome.Win;
            return Outcome.Loss;
        }
    }
}
