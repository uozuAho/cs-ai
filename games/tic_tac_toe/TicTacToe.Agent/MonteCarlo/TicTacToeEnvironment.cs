using System;
using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    /// <summary>
    /// An AI Gym-like interface to tic tac toe
    /// See https://gym.openai.com/docs/
    /// </summary>
    public class TicTacToeEnvironment
    {
        private readonly ITicTacToeAgent _opponent;
        private Board _board;

        public TicTacToeEnvironment(ITicTacToeAgent opponent)
        {
            _opponent = opponent;
            Reset();
        }

        public Board Reset()
        {
            _board = Board.CreateEmptyBoard();

            return _board;
        }

        public void SetState(Board board)
        {
            _board = board;
        }

        public TicTacToeEnvironmentStep Step(TicTacToeAction action)
        {
            try
            {
                _board.Update(action);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            if (!_board.IsValid())
                throw new InvalidOperationException($"Action caused invalid state: '{_board.AsString()}'");

            if (!_board.IsGameOver)
                _board.Update(_opponent.GetAction(this, _board));

            var reward = 0.0;
            if (_board.Winner() == BoardTile.X) reward = 1.0;
            if (_board.Winner() == BoardTile.O) reward = -1.0;

            return new TicTacToeEnvironmentStep
            {
                Board = _board,
                Reward = reward
            };
        }

        public IEnumerable<TicTacToeAction> ActionSpace()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                if (_board.GetTileAt(pos) == BoardTile.Empty)
                {
                    yield return new TicTacToeAction
                    {
                        Position = pos,
                        Tile = BoardTile.X
                    };
                }
            }
        }
    }
}
