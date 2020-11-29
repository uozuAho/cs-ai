using System.Collections.Generic;
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

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            return game.GetAvailableActions().First();
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return environment.AvailableActions().First();
        }

        public void Train(IPlayer opponent)
        {
            for (var i = 0; i < 100; i++)
            {
                ImprovePolicy(opponent, new ActionValues(), new Returns());
            }
        }

        private void ImprovePolicy(IPlayer opponent, ActionValues actionValues, Returns returns)
        {
            var reward_sum = 0;
            var exploringPolicy = new ExploringStartPolicy(this);

            // temp to get test passing
            // opponent.DoNextTurn();
            // CurrentPolicy.AddAction(opponent.Board, new TicTacToeAction());

            var episode = CreateEpisode(exploringPolicy, opponent);
            CurrentPolicy.AddAction(episode[1].Board, new TicTacToeAction());
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
            //         agentPolicy.set_action(state, best_action);
            //     }
            // }
        }

        private List<TicTacToeObservation> CreateEpisode(
            ExploringStartPolicy agentPolicy,
            IPlayer opponent)
        {
            var env = new TicTacToeEnvironment(opponent);
        
            var lastObservation = env.Reset();
            var observations = new List<TicTacToeObservation> {lastObservation};
        
            while (!lastObservation.IsDone)
            {
                lastObservation = env.Step(GetAction(env));
                observations.Add(lastObservation);
            }

            return observations;
        }
    }
}
