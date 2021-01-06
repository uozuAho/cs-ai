using System;
using System.Linq;
using ailib.Utils;
using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents.MonteCarlo
{
    /// <summary>
    /// Monte Carlo learning agent.
    /// - On-policy
    /// - e-greedy
    /// - first-visit
    /// - exploring starts
    /// </summary>
    public class MonteCarloTicTacToeAgent : ITicTacToeAgent
    {
        public BoardTile Tile { get; }

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private readonly Random _random = new();

        private readonly FixedPolicy _currentPolicy = new();
        private ActionValues _actionValues = new();
        private Returns _returns = new();

        public MonteCarloTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            var action = _currentPolicy.HasActionFor(environment.CurrentState)
                ? _currentPolicy.Action(environment.CurrentState)
                : _random.Choice(environment.ActionSpace());

            if (_random.TrueWithProbability(ChanceOfRandomAction))
                action = _random.Choice(environment.ActionSpace());

            return action;
        }

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            var lastNumStates = 0;
            var noNewStatesSeenForXEpisodes = 0;
            _actionValues = new ActionValues();
            _returns = new Returns();

            var maxGames = numGamesLimit ?? 10000;

            for (var i = 0; i < maxGames; i++)
            {
                ImprovePolicy(opponent, _actionValues, _returns);

                if (_currentPolicy.NumStates == lastNumStates)
                    noNewStatesSeenForXEpisodes++;
                else
                {
                    noNewStatesSeenForXEpisodes = 0;
                }

                lastNumStates = _currentPolicy.NumStates;

                // improvement: make this more meaningful. Maybe stop on value change
                // falling below a certain threshold
                if (noNewStatesSeenForXEpisodes == 3000)
                    break;
            }
        }

        public ITicTacToePolicy GetCurrentPolicy(string name, string description)
        {
            // todo: add a persistence layer, don't use serializable stuff directly
            var policy = new SerializableStateActionPolicy(name, description, Tile);

            foreach (var (board, action) in _currentPolicy.AllActions())
            {
                var value = _actionValues.HighestValue(board);
                policy.AddStateAction(board, action.Position, value);
            }

            return policy;
        }

        private void ImprovePolicy(ITicTacToePlayer opponent, ActionValues actionValues, Returns returns)
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
                    _currentPolicy.SetAction(state, bestAction);
                }
            }
        }
    }
}
