using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class MonteCarloTicTacToeAgent : IPlayer
    {
        public BoardTile Tile { get; }

        public TicTacToePolicy CurrentPolicy { get; set; } = new TicTacToePolicy();

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
            var reward_sum = 0.0;
            var exploringPolicy = new ExploringStartPolicy(this);

            var episode = Episode.Generate(exploringPolicy, opponent);
            CurrentPolicy.AddAction(episode.Steps[1].State, new TicTacToeAction());
            foreach (var t in Enumerable.Range(0, episode.Length - 1).Reverse())
            {
                var state = episode.Steps[t].State;
                var action = episode.Steps[t].Action;
                reward_sum += episode.Steps[t + 1].Reward;
                if (episode.TimeOfFirstVisit(state, action) == t)
                {
                    returns.Add(state, action, reward_sum);
                    actionValues.Set(state, action, returns.AverageReturnFrom(state, action));
                    var best_action = actionValues.HighestValueAction(state);
                    // agentPolicy.set_action(state, best_action);
                }
            }
        }
    }
}
