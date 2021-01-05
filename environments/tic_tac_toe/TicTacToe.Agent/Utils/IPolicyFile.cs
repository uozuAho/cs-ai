using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public interface IPolicyFile
    {
        string Name { get; }
        string Description { get; }
        BoardTile Tile { get; }

        ITicTacToePlayer ToPlayer();
    }
}