using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Game
{
    public class TicTacToeGame
    {
        public Board Board { get; private set; }

        private readonly ITicTacToePlayer _player1;
        private readonly ITicTacToePlayer _player2;

        private ITicTacToePlayer CurrentTicTacToePlayer => Board.CurrentPlayer == _player1.Tile ? _player1 : _player2;

        private readonly List<IGameStateObserver> _stateObservers = new();

        public TicTacToeGame(Board board, ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            Board = board;
            _player1 = player1;
            _player2 = player2;

            ThrowIfPlayersInvalid();

            foreach (var player in new[] {_player1, _player2})
            {
                if (player is IGameStateObserver observer)
                    _stateObservers.Add(observer);
            }
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
            var previousState = Board;
            Board = Board.DoAction(action);
            NotifyObservers(previousState, Board);
            return Board;
        }

        private void NotifyObservers(Board previousState, Board currentState)
        {
            foreach (var observer in _stateObservers)
            {
                observer.NotifyStateChanged(previousState, currentState);
            }
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
