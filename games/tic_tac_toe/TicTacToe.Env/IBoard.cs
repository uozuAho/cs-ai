﻿using System.Collections.Generic;

namespace TicTacToe.Game
{
    /// <summary>
    /// Tic tac toe board
    /// </summary>
    /// <remarks>
    ///
    /// # Board positions
    /// 
    /// Board positions are referred to in 1 dimension.
    ///
    /// 0 1 2
    /// 3 4 5
    /// 7 8 9
    ///
    /// </remarks>
    public interface IBoard
    {
        BoardTile CurrentPlayer { get; set; }
        bool IsGameOver { get; }

        IEnumerable<TicTacToeAction> AvailableActions();
        void Update(TicTacToeAction action);
        BoardTile GetTileAt(int pos);
        BoardTile? Winner();

        IBoard Clone();
        bool IsValid();
        bool IsSameStateAs(IBoard otherBoard);
        bool IsFull();

        string ToString();
    }
}