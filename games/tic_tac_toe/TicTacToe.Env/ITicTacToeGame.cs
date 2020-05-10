using System.Collections.Generic;

namespace TicTacToe.Env
{
    public interface ITicTacToeGame
    {
        IBoard Board { get; }
        IEnumerable<TicTacToeAction> GetAvailableActions();
        void DoNextTurn();
        bool IsFinished();
        BoardTile? Winner();
    }
}