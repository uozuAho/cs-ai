using System;
using ailib.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    internal class ExploringStartPolicy : IPlayer
    {
        public BoardTile Tile => _innerPolicy.Tile;

        private bool _isFirstAction;
        private readonly Random _rng;
        private readonly IPlayer _innerPolicy;

        public ExploringStartPolicy(IPlayer innerPolicy)
        {
            _innerPolicy = innerPolicy;
            _isFirstAction = true;
            _rng = new Random();
        }

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            var action = _isFirstAction
                ? _rng.Choice(game.GetAvailableActions())
                : _innerPolicy.GetAction(game);

            _isFirstAction = false;

            return action;
        }
    }
}