using System;

namespace TicTacToe.Env
{
    public enum BoardTile
    {
        Empty,
        X,
        O
    }

    public static class BoardTileExtensions
    {
        public static string AsString(this BoardTile boardTile)
        {
            switch (boardTile)
            {
                case BoardTile.X: return "x";
                case BoardTile.O: return "o";
                case BoardTile.Empty: return " ";
            }
            throw new ArgumentException();
        }

        public static BoardTile Other(this BoardTile boardTile)
        {
            switch (boardTile)
            {
                case BoardTile.X: return BoardTile.O;
                case BoardTile.O: return BoardTile.X;
            }
            throw new ArgumentException();
        }
    }
}