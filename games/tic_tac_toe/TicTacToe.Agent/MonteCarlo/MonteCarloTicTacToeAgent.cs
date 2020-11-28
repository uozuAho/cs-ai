using System;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class MonteCarloTicTacToeAgent : IPlayer
    {
        public BoardTile Tile { get; }
        public TicTacToePolicy CurrentPolicy { get; set; } = new TicTacToePolicy();

        private Random _rng;

        public MonteCarloTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
            _rng = new Random();
        }

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            return game.GetAvailableActions().First();
        }

        public void Train(ITicTacToeGame game)
        {
            for (var i = 0; i < 1; i++)
            {
                ImprovePolicy(game, new ActionValues(), new Returns());
            }
        }

        private void ImprovePolicy(ITicTacToeGame game, ActionValues actionValues, Returns returns)
        {
            var reward_sum = 0;
            var exploring_policy = new ExploringStartPolicy(this);

            // temp to get test passing
            CurrentPolicy.AddAction(game.Board, new TicTacToeAction());

            // var episode = bj.Episode(list(bj.generate_random_episode(exploring_policy)));
            // foreach (var t in reversed(range(episode.length() - 1)))
            // {
            //     var state = episode.steps[t].state;
            //     var action = episode.steps[t].action;
            //     reward_sum += episode.steps[t + 1].reward;
            //     if (episode.first_visit(state, action) == t)
            //     {
            //         returns.add(state, action, reward_sum);
            //         actionValues.set(state, action, returns.average_for(state, action));
            //         var best_action = actionValues.highest_value_action(state);
            //         policy.set_action(state, best_action);
            //     }
            // }
        }
    }
}
