using System;
using System.Linq;
using ailib.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class MonteCarloTicTacToeAgent
    {
        public BoardTile Tile { get; }

        public TicTacToeMutablePolicy CurrentMutablePolicy { get; set; } = new();

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private readonly Random _random = new();

        public MonteCarloTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
        }

        public IPlayer ToFixedPolicyPlayer()
        {
            return CurrentMutablePolicy.ToPlayer(Tile);
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            var action = CurrentMutablePolicy.HasActionFor(environment.CurrentState)
                ? CurrentMutablePolicy.Action(environment.CurrentState)
                : _random.Choice(environment.ActionSpace());

            if (_random.TrueWithProbability(ChanceOfRandomAction))
                action = _random.Choice(environment.ActionSpace());

            return action;
        }

        public void Train(ITicTacToeAgent opponent)
        {
            var lastNumStates = 0;
            var noNewStatesSeenForXEpisodes = 0;
            var actionValues = new ActionValues();
            var returns = new Returns();

            for (var i = 0; i < 10000; i++)
            {
                ImprovePolicy(opponent, actionValues, returns);

                if (CurrentMutablePolicy.NumStates == lastNumStates)
                    noNewStatesSeenForXEpisodes++;
                else
                {
                    noNewStatesSeenForXEpisodes = 0;
                }

                lastNumStates = CurrentMutablePolicy.NumStates;

                // improvement: make this more meaningful. Maybe stop on value change
                // falling below a certain threshold
                if (noNewStatesSeenForXEpisodes == 3000)
                    break;
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

                if (action == null) continue;

                if (episode.TimeOfFirstVisit(state, action) == t)
                {
                    returns.Add(state, action, rewardSum);
                    actionValues.Set(state, action, returns.AverageReturnFrom(state, action));
                    var bestAction = actionValues.HighestValueAction(state);
                    CurrentMutablePolicy.SetAction(state, bestAction);
                }
            }
        }
    }
}
