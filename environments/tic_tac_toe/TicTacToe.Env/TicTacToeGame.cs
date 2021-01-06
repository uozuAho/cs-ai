using System;
using System.Linq;

namespace TicTacToe.Game
{
    public class TicTacToeGame
    {
        public Board Board { get; private set; }

        private readonly ITicTacToePlayer _player1;
        private readonly ITicTacToePlayer _player2;

        private ITicTacToePlayer CurrentTicTacToePlayer => Board.CurrentPlayer == _player1.Tile ? _player1 : _player2;

        public TicTacToeGame(Board board, ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            Board = board;
            _player1 = player1;
            _player2 = player2;

            ThrowIfPlayersInvalid();
        }

        public void Run()
        {
            while (!IsFinished())
            {
                DoNextTurn();
            }
        }

        public Board DoNextTurn()
        {
            var action = CurrentTicTacToePlayer.GetAction(Board);
            Board = Board.DoAction(action);
            return Board;
        }

        public bool IsFinished()
        {
            return Winner().HasValue || !Board.AvailableActions().Any();
        }

        public BoardTile? Winner()
        {
            return Board.Winner();
        }

        private void ThrowIfPlayersInvalid()
        {
            var playerTiles = new[] {_player1.Tile, _player2.Tile};
            if (playerTiles.Count(t => t == BoardTile.O) != 1)
                throw new InvalidOperationException($"Must have one ticTacToePlayer with tile {BoardTile.O}");
            if (playerTiles.Count(t => t == BoardTile.X) != 1)
                throw new InvalidOperationException($"Must have one ticTacToePlayer with tile {BoardTile.X}");
        }
    }
}
