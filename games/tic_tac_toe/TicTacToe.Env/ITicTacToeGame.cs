using System.Collections.Generic;

namespace TicTacToe.Game
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