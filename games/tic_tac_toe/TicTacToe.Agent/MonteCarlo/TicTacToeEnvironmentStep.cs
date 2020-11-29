using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToeEnvironmentStep
    {
        public Board Board { get; set; } = Board.CreateEmptyBoard();
        public double Reward { get; set; }
        public bool IsDone => Board.IsGameOver;
    }
}