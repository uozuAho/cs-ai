namespace TicTacToe.Env
{
    public interface IPlayer
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(ITicTacToeGame game);
    }
}