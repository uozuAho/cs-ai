using TicTacToe.Env;

namespace TicTacToe.Agent
{
    public class PTableAgentConfig
    {
        public BoardTile PlayerTile { get; set; }
        public double RandomActionProbability { get; set; }
        public double LearningRate { get; set; }
    }
}