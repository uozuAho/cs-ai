using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    internal record EpisodeStep
    {
        /// <summary>
        /// Reward for the previous action
        /// </summary>
        public double Reward { get; init; }

        /// <summary>
        /// Current state
        /// </summary>
        public IBoard State { get; init; }

        /// <summary>
        /// Action taken from the current state
        /// </summary>
        public TicTacToeAction Action { get; init; }
    }
}