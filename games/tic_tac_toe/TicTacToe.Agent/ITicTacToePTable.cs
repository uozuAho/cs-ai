using TicTacToe.Env;

namespace TicTacToe.Agent
{
    public interface ITicTacToePTable
    {
        double GetWinProbability(IBoard board);
        void UpdateWinProbability(IBoard board, double winProbability);
    }
}