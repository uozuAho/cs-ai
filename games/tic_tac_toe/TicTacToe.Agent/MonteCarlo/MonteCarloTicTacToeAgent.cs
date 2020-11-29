using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class MonteCarloTicTacToeAgent : IPlayer
    {
        public BoardTile Tile { get; }

        public TicTacToePolicy CurrentPolicy { get; set; } = new();

        public MonteCarloTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
        }

        public TicTacToeAction GetAction(IBoard board)
        {
            return board.AvailableActions().First();
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return environment.ActionSpace().First();
        }

        public void Train(ITicTacToeAgent opponent)
        {
            for (var i = 0; i < 100; i++)
            {
                ImprovePolicy(opponent, new ActionValues(), new Returns());
            }
        }

        private void ImprovePolicy(ITicTacToeAgent opponent, ActionValues actionValues, Returns returns)
        {
            var rewardSum = 0.0;
            var exploringPolicy = new ExploringStartPolicy(this);

            var episode = Episode.Generate(exploringPolicy, opponent);

            foreach (var t in Enumerable.Range(0, episode.Length - 1).Reverse())
            {
                var state = episode.Steps[t].State;
                var action = episode.Steps[t].Action;
                rewardSum += episode.Steps[t + 1].Reward;
                if (episode.TimeOfFirstVisit(state, action) == t)
                {
                    returns.Add(state, action, rewardSum);
                    actionValues.Set(state, action, returns.AverageReturnFrom(state, action));
                    var bestAction = actionValues.HighestValueAction(state);
                    CurrentPolicy.SetAction(state, bestAction);
                }
            }
        }
    }
}
