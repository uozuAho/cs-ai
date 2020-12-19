using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record SerializableStateAction(SerializableBoard Board, SerializableTicTacToeAction Action);
}