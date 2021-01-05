using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record StateAction(string Board, double Value, int Action);

    public record StateActionPolicy(
        string Name,
        string Description,
        BoardTile Tile,
        StateAction[] Actions) : IPolicyFile
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