namespace TicTacToe.Game
{
    public record TicTacToeAction
    {
        public BoardTile Tile { get; init; }
        public int Position { get; init; }

        public override string ToString()
        {
            return $"{Tile} {Position}";
        }
    }
}