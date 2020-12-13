using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
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

        public TicTacToeAction GetAction(Board board)
        {
            return _actionMap.ActionFor(board);
        }
    }
}