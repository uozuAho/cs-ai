using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    internal class EpisodeStep
    {
        /// <summary>
        /// Reward for the previous action
        /// </summary>
        public double Reward { get; set; }

        /// <summary>
        /// Current state
        /// </summary>
        public Board State { get; set; }

        /// <summary>
        /// Action taken from the current state
        /// </summary>
        public TicTacToeAction Action { get; set; }
    }
}