using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class TicTacToeFixedPolicyPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }
        private readonly FixedPolicy _actionMap;

        public TicTacToeFixedPolicyPlayer(PolicyFile policy)
        {
            Tile = policy.Tile;
            _actionMap = policy.ToPolicy();
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _actionMap.Action(board);
        }
    }
}