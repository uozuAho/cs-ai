using System;
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

        private TicTacToePTableLazy _values;

        public Td0Agent(BoardTile tile)
        {
            Tile = tile;
            _values = new TicTacToePTableLazy(Tile);
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return ShouldPickBestAction()
                ? BestAction(environment.CurrentState)
                : RandomAction(environment);
        }

        public BoardActionMap GetCurrentPolicy()
        {
            var map = new BoardActionMap();
            foreach (var (board, _) in _values.All())
            {
                var bestAction = BestAction(board);
                map.SetAction(board, bestAction);
            }

            return map;
        }

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            var maxGames = numGamesLimit ?? 10000;
            var env = new TicTacToeEnvironment(opponent);
            _values = new TicTacToePTableLazy(Tile);

            // todo: break when td error drops below threshold
            for (var i = 0; i < maxGames; i++)
            {
                env.Reset();

                while (!env.CurrentState.IsGameOver)
                {
                    var board = env.CurrentState;
                    var currentValue = _values.GetWinProbability(board);
                    var step = env.Step(GetAction(env));
                    var nextBoard = env.CurrentState;

                    var updatedValue =
                        _values.GetWinProbability(env.CurrentState)
                        + LearningRate * (step.Reward + _values.GetWinProbability(nextBoard) - currentValue);

                    _values.UpdateWinProbability(board, updatedValue);
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
                .MaxBy(b => _values.GetWinProbability(b.board))
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
