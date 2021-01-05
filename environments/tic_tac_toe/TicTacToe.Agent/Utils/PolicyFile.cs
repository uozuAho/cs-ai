using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record PolicyFile(
        string Name,
        string Description,
        BoardTile Tile,
        PolicyFileAction[] Actions) : IPolicyFile
    {
        public ITicTacToePlayer ToPlayer()
        {
            return new TicTacToeFixedPolicyPlayer(ToPolicy(), Tile);
        }

        private FixedPolicy ToPolicy()
        {
            var policy = new FixedPolicy();

            foreach (var (boardString, _, position) in Actions)
            {
                var board = Board.CreateFromString(boardString, Tile);
                var action = new TicTacToeAction { Tile = Tile, Position = position };
                policy.SetAction(board, action);
            }

            return policy;
        }
    }
}