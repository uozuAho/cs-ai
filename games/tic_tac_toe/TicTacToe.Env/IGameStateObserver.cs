namespace TicTacToe.Game
{
    public interface IGameStateObserver
    {
        void NotifyStateChanged(IBoard previousState, IBoard currentState);
    }
}