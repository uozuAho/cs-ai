namespace TicTacToe.Game
{
    public interface IPlayer
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(ITicTacToeGame game);
    }
}