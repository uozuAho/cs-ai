using System;
using System.Diagnostics;
using System.Linq;
using ailib.Utils;
using MoreLinq;
using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class NStepAgent
    {
        public BoardTile Tile { get; }

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private const double LearningRate = 0.05;
        private readonly Random _random = new();

        private StateValueTable _values;

        public NStepAgent(BoardTile tile)
        {
            Tile = tile;
            _values = new StateValueTable(Tile);
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return ShouldDoExploratoryAction()
                ? RandomAction(environment)
                : BestAction(environment.CurrentState);
        }

        public StateValueTable GetCurrentStateValues()
        {
            return _values;
        }

        public ITicTacToePolicy GetCurrentValues(string name, string description)
        {
            var policy = new SerializableStateValueTable(name, description, Tile);
            policy.SetStateValues(_values);
            return policy;
        }

        public void SaveTrainedValues(string agentName, string path)
        {
            PolicyFileIo.Save(_values, agentName, "", path);
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
                env.Reset();
                Board? previousAfterstate = null;

                while (!env.CurrentState.IsGameOver)
                {
                    var isExploratoryAction = ShouldDoExploratoryAction();
                    var action = isExploratoryAction ? RandomAction(env) : BestAction(env.CurrentState);
                    var afterstate = env.CurrentState.DoAction(action);
                    env.Step(action);

                    if (previousAfterstate != null && !isExploratoryAction)
                    {
                        var tdError = _values.Value(afterstate) - _values.Value(previousAfterstate);
                        var updatedValue = _values.Value(previousAfterstate) + LearningRate * tdError;

                        _values.SetValue(previousAfterstate, updatedValue);
                    }
                    previousAfterstate = afterstate;
                }
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

        private bool ShouldDoExploratoryAction()
        {
            return _random.NextDouble() < ChanceOfRandomAction;
        }
    }
}
