using TicTacToe.Env;

namespace TicTacToe.Agent
{
    public interface IGameStateObserver
    {
        void NotifyStateChanged(IBoard previousState, IBoard currentState);
    }
}