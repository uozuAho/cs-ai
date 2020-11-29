using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToePolicyPlayer : IPlayer
    {
        public BoardTile Tile { get; }
        private readonly Dictionary<string, TicTacToeAction> _actionMap;

        // todo: make dict board: action
        public TicTacToePolicyPlayer(BoardTile tile, Dictionary<string, TicTacToeAction> actionMap)
        {
            _actionMap = actionMap;
            Tile = tile;
        }

        public TicTacToeAction GetAction(IBoard board)
        {
            return _actionMap[board.AsString()];
        }
    }
}