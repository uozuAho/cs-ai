namespace TicTacToe.Game
{
    public interface IGameStateObserver
    {
        void NotifyStateChanged(Board previousState, Board currentState);
    }
}