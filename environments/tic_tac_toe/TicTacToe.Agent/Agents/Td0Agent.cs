using System;
using System.Diagnostics;
using System.Linq;
using ailib.Utils;
using MoreLinq;
using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    /// <summary>
    /// One-step temporal difference learning agent. On-policy. Learns afterstate-value
    /// function, ie. the value of the state after the agent's move gets updated. This
    /// negates the need to model the opponent's behaviour, as it becomes part of the
    /// value function.
    /// </summary>
    public class Td0Agent : ITicTacToeAgent
    {
        public BoardTile Tile { get; }

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private const double LearningRate = 0.05;
        private readonly Random _random = new();

        private StateValueTable _values;

        public Td0Agent(BoardTile tile)
        {
            Tile = tile;
            _values = new StateValueTable(Tile);
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return ShouldPickBestAction()
                ? BestAction(environment.CurrentState)
                : RandomAction(environment);
        }

        public StateValueTable GetCurrentStateValues()
        {
            return _values;
        }

        public ITicTacToePolicy GetCurrentPolicy(string name, string description)
        {
            var policy = new SerializableStateValuePolicy(name, description, Tile);
            policy.SetStateValues(_values);
            return policy;
        }

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            var gameCount = 0;
            var maxGames = numGamesLimit ?? 10000;
            var env = new TicTacToeEnvironment(opponent);
            _values = new StateValueTable(Tile);

            var stopwatch = Stopwatch.StartNew();

            for (; gameCount < maxGames; gameCount++)
            {
                var maxTdError = 0.0;
                env.Reset();
                Board? previousAfterstate = null;

                while (!env.CurrentState.IsGameOver)
                {
                    var action = GetAction(env);
                    var afterstate = env.CurrentState.DoAction(action);
                    var step = env.Step(GetAction(env));

                    if (previousAfterstate != null)
                    {
                        var tdError = step.Reward + _values.Value(afterstate) - _values.Value(previousAfterstate);
                        var updatedValue = _values.Value(afterstate) + LearningRate * tdError;

                        if (Math.Abs(tdError - maxTdError) > maxTdError)
                            maxTdError = Math.Abs(tdError - maxTdError);

                        _values.SetValue(afterstate, updatedValue);
                    }

                    previousAfterstate = afterstate;
                }

                if (maxTdError < 0.01) break;
            }

            Console.WriteLine($"Played {gameCount} games in {stopwatch.ElapsedMilliseconds} ms");
        }

        private TicTacToeAction BestAction(Board board)
        {
            return board
                .AvailableActions()
                .Select(a => new
                {
                    action = a,
                    board = board.DoAction(a)
                })
                .MaxBy(b => _values.Value(b.board))
                .First().action;
        }

        private TicTacToeAction RandomAction(TicTacToeEnvironment env)
        {
            return _random.Choice(env.ActionSpace());
        }

        private bool ShouldPickBestAction()
        {
            return _random.NextDouble() > ChanceOfRandomAction;
        }
    }
}
