namespace TicTacToe.Game
{
    public interface ITicTacToePlayer
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(Board board);
    }
}