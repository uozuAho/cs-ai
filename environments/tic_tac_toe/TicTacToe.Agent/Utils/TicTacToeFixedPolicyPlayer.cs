using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class TicTacToeFixedPolicyPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }
        private readonly FixedPolicy _policy;

        private TicTacToeFixedPolicyPlayer(FixedPolicy policy, BoardTile tile)
        {
            Tile = tile;
            _policy = policy;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _policy.Action(board);
        }

        public static ITicTacToePlayer FromPolicyFile(PolicyFile policyFile)
        {
            return new TicTacToeFixedPolicyPlayer(policyFile.ToPolicy(), policyFile.Tile);
        }
    }
}