namespace TicTacToe.Env
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
        void Update(TicTacToeAction action);
        BoardTile GetTileAt(int pos);
        BoardTile? Winner();

        /// <summary>
        /// A string representation of the board
        /// </summary>
        string AsString();

        IBoard Clone();
        bool IsValid();
        bool IsSameStateAs(IBoard otherBoard);
        bool IsFull();
    }
}