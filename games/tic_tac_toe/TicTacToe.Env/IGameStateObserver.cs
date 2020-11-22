namespace TicTacToe.Env
{
    public interface IGameStateObserver
    {
        void NotifyStateChanged(IBoard previousState, IBoard currentState);
    }
}