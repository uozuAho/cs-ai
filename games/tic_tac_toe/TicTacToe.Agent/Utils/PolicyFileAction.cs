namespace TicTacToe.Agent.Utils
{
    public record PolicyFileAction(
        string Board,
        double Value,
        int Action);
}