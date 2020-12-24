using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class TicTacToeFixedPolicyPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }
        private readonly BoardActionMap _actionMap;

        public TicTacToeFixedPolicyPlayer(PolicyFile policy)
        {
            Tile = policy.Tile;
            _actionMap = policy.BuildActionMap();
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _actionMap.ActionFor(board);
        }
    }
}