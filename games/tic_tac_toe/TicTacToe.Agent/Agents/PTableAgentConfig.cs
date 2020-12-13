using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class PTableAgentConfig
    {
        public BoardTile PlayerTile { get; set; }
        public double RandomActionProbability { get; set; }
        public double LearningRate { get; set; }
    }
}