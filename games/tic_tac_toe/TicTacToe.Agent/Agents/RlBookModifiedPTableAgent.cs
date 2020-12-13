using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    /// <summary>
    /// A copy of the RL book's p table agent, except that it updates the p table after _all_ state changes.
    /// It learns to win all games against <see cref="FirstAvailableSlotAgent"/>, once random actions are
    /// turned off.
    /// </summary>
    public class RlBookModifiedPTableAgent : ITicTacToePlayer, IGameStateObserver
    {
        public BoardTile Tile { get; }

        private readonly ITicTacToePTable _pTable;
        private readonly PTableAgentConfig _config;
        private readonly Random _rng;

        public RlBookModifiedPTableAgent(ITicTacToePTable pTable, PTableAgentConfig config)
        {
            _pTable = pTable;
            Tile = config.PlayerTile;
            _config = config;
            _rng = new Random();
        }

        public static RlBookModifiedPTableAgent CreateDefaultAgent(BoardTile playerTile)
        {
            return new RlBookModifiedPTableAgent(new TicTacToePTableLazy(playerTile), new PTableAgentConfig
            {
                LearningRate = 0.1,
                PlayerTile = playerTile,
                RandomActionProbability = 0.25
            });
        }

        public TicTacToeAction GetAction(Board board)
        {
            var availableActions = board.AvailableActions().ToList();
            if (availableActions.Count == 0)
                throw new InvalidOperationException("no actions");

            return ShouldPickBestAction()
                ? FindBestAction(board, availableActions)
                : PickRandomAction(availableActions);
        }

        public void NotifyStateChanged(Board previousState, Board currentState)
        {
            var previousStatePWin = CalculateNewProbability(previousState, currentState);
            _pTable.UpdateWinProbability(previousState, previousStatePWin);
        }

        // P(State(t)) + alpha[P(State(t + 1)) - P(State(t))]
        private double CalculateNewProbability(Board board, Board nextBoard)
        {
            var Pwin = _pTable.GetWinProbability(board);
            var Pwin_next = _pTable.GetWinProbability(nextBoard);
            var alpha = _config.LearningRate;

            return Pwin + alpha * (Pwin_next - Pwin);
        }

        private bool ShouldPickBestAction()
        {
            return _rng.NextDouble() > _config.RandomActionProbability;
        }

        private TicTacToeAction FindBestAction(Board board, IReadOnlyList<TicTacToeAction> availableActions)
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

        private static Board CreateNewState(Board board, TicTacToeAction action)
        {
            return board.DoAction(action);
        }

        private TicTacToeAction PickRandomAction(IReadOnlyList<TicTacToeAction> availableActions)
        {
            var idx = _rng.Next(0, availableActions.Count);
            return availableActions[idx];
        }
    }
}
