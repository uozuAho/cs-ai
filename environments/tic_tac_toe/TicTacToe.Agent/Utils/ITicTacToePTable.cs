using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public interface ITicTacToePTable
    {
        double GetWinProbability(Board board);
        void UpdateWinProbability(Board board, double winProbability);
    }
}