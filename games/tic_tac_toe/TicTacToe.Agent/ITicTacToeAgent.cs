using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public interface ITicTacToeAgent
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(TicTacToeEnvironment environment, Board board);
    }
}