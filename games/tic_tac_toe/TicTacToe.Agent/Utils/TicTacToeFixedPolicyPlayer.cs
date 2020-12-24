using System;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class TicTacToeFixedPolicyPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }
        private readonly BoardActionMap _actionMap;

        public TicTacToeFixedPolicyPlayer(BoardTile tile, BoardActionMap actionMap)
        {
            _actionMap = actionMap;
            Tile = tile;
        }

        public TicTacToeFixedPolicyPlayer(BoardTile tile, PolicyFile policy)
        {
            if (tile != policy.Tile) throw new InvalidOperationException("tile must match policy tile");

            Tile = policy.Tile;
            _actionMap = policy.BuildActionMap();
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _actionMap.ActionFor(board);
        }
    }
}