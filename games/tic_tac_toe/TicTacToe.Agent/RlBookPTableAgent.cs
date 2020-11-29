using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    /// <summary>
    /// The tic-tac-toe playing agent as described in the RL book, 1.5
    /// "An Extended Example: Tic-Tac-Toe". I can't get this to work.
    /// The only way this agent wins is if RandomActionProbability > 0,
    /// even after I've trained it with 1000s of games. I think the problem
    /// is in updating the p-table only after non-random moves by this agent.
    /// This means the table is not updated when the agent loses, which I think
    /// is critical information...
    /// </summary>
    public class RlBookPTableAgent : IPlayer
    {
        public BoardTile Tile { get; }

        private readonly ITicTacToePTable _pTable;
        private readonly PTableAgentConfig _config;
        private readonly Random _rng;

        public RlBookPTableAgent(ITicTacToePTable pTable, PTableAgentConfig config)
        {
            _pTable = pTable;
            Tile = config.PlayerTile;
            _config = config;
            _rng = new Random();
        }

        public static RlBookPTableAgent CreateDefaultAgent(BoardTile playerTile)
        {
            return new RlBookPTableAgent(new TicTacToePTableLazy(playerTile), new PTableAgentConfig
            {
                LearningRate = 0.1,
                PlayerTile = playerTile,
                RandomActionProbability = 0.25
            });
        }

        public TicTacToeAction GetAction(IBoard board)
        {
            var availableActions = board.AvailableActions().ToList();
            if (availableActions.Count == 0)
                throw new InvalidOperationException("no actions");

            if (ShouldPickBestAction())
            {
                var action = FindBestAction(board, availableActions);
                UpdatePTable(board, action);
                return action;
            }
            else
            {
                var action = PickRandomAction(availableActions);
                // Deviation from the book's algorithm: update p table after random moves.
                // Doesn't help :(
                // UpdatePTable(board, action);
                return action;
            }
        }

        private void UpdatePTable(IBoard board, TicTacToeAction action)
        {
            var newProbability = CalculateNewProbability(board, action);
            _pTable.UpdateWinProbability(board, newProbability);
        }

        // P(State(t)) + alpha[P(State(t + 1)) - P(State(t))]
        private double CalculateNewProbability(IBoard board, TicTacToeAction action)
        {
            var nextBoard = board.Clone();
            nextBoard.Update(action);
            var Pt = _pTable.GetWinProbability(board);
            var Pt_next = _pTable.GetWinProbability(nextBoard);
            var alpha = _config.LearningRate;

            return Pt + alpha * (Pt_next - Pt);
        }

        private bool ShouldPickBestAction()
        {
            return _rng.NextDouble() > _config.RandomActionProbability;
        }

        private TicTacToeAction FindBestAction(IBoard board, IReadOnlyList<TicTacToeAction> availableActions)
        {
            var highestProb = 0.0;
            var highestProbAction = availableActions[0];

            foreach (var action in availableActions)
            {
                Debug.Assert(action.Tile == Tile);
                var nextState = CreateNewState(board, action);
                var nextStateProb = _pTable.GetWinProbability(nextState);
                if (nextStateProb > highestProb)
                {
                    highestProb = nextStateProb;
                    highestProbAction = action;
                }
            }

            return highestProbAction;
        }

        private static IBoard CreateNewState(IBoard board, TicTacToeAction action)
        {
            var newBoard = board.Clone();
            newBoard.Update(action);
            return newBoard;
        }

        private TicTacToeAction PickRandomAction(IReadOnlyList<TicTacToeAction> availableActions)
        {
            var idx = _rng.Next(0, availableActions.Count);
            return availableActions[idx];
        }
    }
}
