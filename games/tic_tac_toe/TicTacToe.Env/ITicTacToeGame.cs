namespace TicTacToe.Game
{
    public interface ITicTacToeGame
    {
        IBoard Board { get; }
        void DoNextTurn();
        bool IsFinished();
        BoardTile? Winner();
    }
}