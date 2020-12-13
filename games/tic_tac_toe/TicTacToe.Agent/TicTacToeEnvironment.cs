using System;
using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    /// <summary>
    /// An AI Gym-like interface to tic tac toe
    /// See https://gym.openai.com/docs/
    /// </summary>
    public class TicTacToeEnvironment
    {
        public Board CurrentState { get; private set; } = Board.CreateEmptyBoard(BoardTile.X);

        private readonly ITicTacToeAgent _opponent;

        public TicTacToeEnvironment(ITicTacToeAgent opponent)
        {
            _opponent = opponent;
            Reset();
        }

        public Board Reset()
        {
            CurrentState = Board.CreateEmptyBoard(_opponent.Tile.Other());

            return CurrentState.Clone();
        }

        public void SetState(Board board)
        {
            CurrentState = board;
        }

        public TicTacToeEnvironmentStep Step(TicTacToeAction action)
        {
            try
            {
                DoAction(action);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            if (!CurrentState.IsValid())
                throw new InvalidOperationException($"Action caused invalid state: '{CurrentState}'");

            if (!CurrentState.IsGameOver)
                DoAction(_opponent.GetAction(this));

            var reward = 0.0;
            if (CurrentState.Winner() == BoardTile.X) reward = 1.0;
            if (CurrentState.Winner() == BoardTile.O) reward = -1.0;

            return new TicTacToeEnvironmentStep
            {
                Board = CurrentState,
                Reward = reward
            };
        }

        public IEnumerable<TicTacToeAction> ActionSpace()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                if (CurrentState.GetTileAt(pos) == BoardTile.Empty)
                {
                    yield return new TicTacToeAction
                    {
                        Position = pos,
                        Tile = BoardTile.X
                    };
                }
            }
        }

        private void DoAction(TicTacToeAction action)
        {
            CurrentState = CurrentState.DoAction(action);
        }
    }
}
