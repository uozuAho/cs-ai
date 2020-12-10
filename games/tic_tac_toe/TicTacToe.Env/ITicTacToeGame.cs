namespace TicTacToe.Game
{
    public interface ITicTacToeGame
    {
        Board Board { get; }
        void DoNextTurn();
        bool IsFinished();
        BoardTile? Winner();
    }
}