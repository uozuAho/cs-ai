namespace TicTacToe.Game
{
    public record SerializableTicTacToeAction
    {
        public int Position { get; init; }
        public BoardTile Tile { get; init; }

        public static SerializableTicTacToeAction FromAction(TicTacToeAction action)
        {
            return new()
            {
                Position = action.Position,
                Tile = action.Tile
            };
        }

        public TicTacToeAction ToAction()
        {
            return new() {Position = Position, Tile = Tile};
        }
    }
}