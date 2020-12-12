using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Game
{
    public class TicTacToeGame
    {
        public Board Board { get; private set; }

        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        private IPlayer CurrentPlayer => Board.CurrentPlayer == _player1.Tile ? _player1 : _player2;

        private readonly List<IGameStateObserver> _stateObservers = new();

        public TicTacToeGame(Board board, IPlayer player1, IPlayer player2)
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
            var action = CurrentPlayer.GetAction(Board);
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
                throw new InvalidOperationException($"Must have one player with tile {BoardTile.O}");
            if (playerTiles.Count(t => t == BoardTile.X) != 1)
                throw new InvalidOperationException($"Must have one player with tile {BoardTile.X}");
        }
    }
}
