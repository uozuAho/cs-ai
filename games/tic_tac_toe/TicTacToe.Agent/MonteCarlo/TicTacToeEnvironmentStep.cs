using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public record TicTacToeEnvironmentStep
    {
        public IBoard Board { get; init; }
        public double Reward { get; init; }
        public bool IsDone => Board.IsGameOver;
    }
}