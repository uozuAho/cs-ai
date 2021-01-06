using TicTacToe.Game;

namespace TicTacToe.Agent.Storage
{
    public record SerializableStateAction(SerializableBoard Board, SerializableTicTacToeAction Action);
}