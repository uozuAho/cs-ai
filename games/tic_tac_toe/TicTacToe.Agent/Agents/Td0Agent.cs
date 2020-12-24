using System;
using System.Collections.Generic;
using System.Linq;
using ailib.Utils;
using MoreLinq;
using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    /// <summary>
    /// One-step temporal difference learning agent. On-policy.
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

        public FixedPolicy GetCurrentPolicy()
        {
            var policy = new FixedPolicy();

            foreach (var (board, _) in _values.All()
                .Where(bv => bv.Item1.CurrentPlayer == Tile)
                .Where(bv => !bv.Item1.IsGameOver))
            {
                var bestAction = BestAction(board);
                policy.SetAction(board, bestAction);
            }

            return policy;
        }

        public PolicyFile GetCurrentPolicyFile(string name, string description)
        {
            var actions = new List<PolicyFileAction>();

            foreach (var (board, value) in _values.All()
                .Where(bv => bv.Item1.CurrentPlayer == Tile)
                .Where(bv => !bv.Item1.IsGameOver))
            {
                var bestAction = BestAction(board);
                actions.Add(new PolicyFileAction(board.ToString(), value, bestAction.Position));
            }

            return new PolicyFile(name, description, Tile, actions.ToArray());
        }

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            var maxGames = numGamesLimit ?? 10000;
            var env = new TicTacToeEnvironment(opponent);
            _values = new StateValueTable(Tile);

            // todo: break when td error drops below threshold
            for (var i = 0; i < maxGames; i++)
            {
                env.Reset();

                while (!env.CurrentState.IsGameOver)
                {
                    var currentBoard = env.CurrentState.Clone();
                    var currentValue = _values.Value(currentBoard);
                    var step = env.Step(GetAction(env));
                    var nextBoard = env.CurrentState;

                    var updatedValue =
                        _values.Value(currentBoard)
                        + LearningRate * (step.Reward + _values.Value(nextBoard) - currentValue);

                    _values.SetValue(currentBoard, updatedValue);
                }
            }
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
