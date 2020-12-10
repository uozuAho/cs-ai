using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public record TicTacToeEnvironmentStep
    {
        public IBoard Board { get; init; } = Game.Board.CreateEmptyBoard();
        public double Reward { get; init; }
        public bool IsDone => Board.IsGameOver;
    }
}