using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public interface ITicTacToePolicy
    {
        string Name { get; }
        string Description { get; }
        BoardTile Tile { get; }
        PolicyFileType Type { get; }

        ITicTacToePlayer ToPlayer();
    }
}