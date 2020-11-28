using System;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToeEnvironment
    {
        private Board _board;

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

            var reward = 0.0;
            if (_board.Winner() == BoardTile.X) reward = 1.0;
            if (_board.Winner() == BoardTile.O) reward = -1.0;

            return new TicTacToeObservation
            {
                Board = _board,
                Reward = reward
            };
        }
    }
}
