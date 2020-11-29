using System;
using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    // agent always has X tiles
    public class TicTacToeEnvironment
    {
        private readonly IPlayer _opponent;
        private Board _board;

        public TicTacToeEnvironment(IPlayer opponent)
        {
            _opponent = opponent;
            Reset();
        }

        public TicTacToeObservation Reset()
        {
            _board = Board.CreateEmptyBoard();

            return new TicTacToeObservation {Board = _board};
        }

        public void SetState(Board board)
        {
            _board = board;
        }

        public TicTacToeObservation Step(TicTacToeAction action)
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

            if (!_board.Winner().HasValue)
                _board.Update(_opponent.GetAction(_board));

            var reward = 0.0;
            if (_board.Winner() == BoardTile.X) reward = 1.0;
            if (_board.Winner() == BoardTile.O) reward = -1.0;

            return new TicTacToeObservation
            {
                Board = _board,
                Reward = reward
            };
        }

        public IEnumerable<TicTacToeAction> AvailableActions()
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
