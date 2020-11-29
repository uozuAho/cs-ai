using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public interface ITicTacToeAgent
    {
        TicTacToeAction GetAction(TicTacToeEnvironment environment, TicTacToeObservation lastObservation);
    }
}