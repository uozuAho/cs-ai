using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public interface ITicTacToePTable
    {
        double GetWinProbability(Board board);
        void UpdateWinProbability(Board board, double winProbability);
    }
}