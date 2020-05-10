using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Agent;

namespace TicTacToe.Env
{
    public class TicTacToeGame : ITicTacToeGame
    {
        public IBoard Board { get; }

        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        private IPlayer _currentPlayer;
        private readonly List<IGameStateObserver> _stateObservers = new List<IGameStateObserver>();

        public TicTacToeGame(IBoard board, IPlayer player1, IPlayer player2)
        {
            Board = board;
            _player1 = player1;
            _player2 = player2;
            _currentPlayer = player1;

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

        public void DoNextTurn()
        {
            var action = _currentPlayer.GetAction(this);
            var previousState = Board.Clone();
            Board.Update(action);
            NotifyObservers(previousState, Board);
            SwitchCurrentPlayer();
        }

        private void NotifyObservers(IBoard previousState, IBoard currentState)
        {
            foreach (var observer in _stateObservers)
            {
                observer.NotifyStateChanged(previousState, currentState);
            }
        }

        public bool IsFinished()
        {
            return Winner().HasValue || !GetAvailableActions().Any();
        }

        public BoardTile? Winner()
        {
            return Board.Winner();
        }

        public IEnumerable<TicTacToeAction> GetAvailableActions()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                if (Board.GetTileAt(pos) == BoardTile.Empty)
                {
                    yield return new TicTacToeAction
                    {
                        Position = pos,
                        Tile = _currentPlayer.Tile
                    };
                }
            }
        }

        private void SwitchCurrentPlayer()
        {
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
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
