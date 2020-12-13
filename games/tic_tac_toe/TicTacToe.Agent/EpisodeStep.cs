using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public record EpisodeStep
    {
        /// <summary>
        /// Reward for the previous action
        /// </summary>
        public double Reward { get; init; }

        /// <summary>
        /// Current state
        /// </summary>
        public Board State { get; init; } = Board.CreateEmptyBoard();

        /// <summary>
        /// Action taken from the current state
        /// </summary>
        public TicTacToeAction? Action { get; init; }
    }
}