using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class TicTacToeFixedPolicyPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }
        private readonly FixedPolicy _policy;

        public TicTacToeFixedPolicyPlayer(FixedPolicy policy, BoardTile tile)
        {
            Tile = tile;
            _policy = policy;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _policy.Action(board);
        }
    }
}
