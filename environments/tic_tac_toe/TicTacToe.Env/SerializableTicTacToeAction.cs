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
    }
}