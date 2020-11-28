using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToeObservation
    {
        public Board Board { get; set; } = Board.CreateEmptyBoard();
        public double Reward { get; set; }
        public bool IsDone => Board.Winner().HasValue;
    }
}