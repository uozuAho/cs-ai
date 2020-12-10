using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public record TicTacToeEnvironmentStep
    {
        public Board Board { get; init; } = Board.CreateEmptyBoard();
        public double Reward { get; init; }
        public bool IsDone => Board.IsGameOver;
    }
}