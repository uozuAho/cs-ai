using System;

namespace TicTacToe.Game
{
    public record SerializableBoard
    {
        public string? Board { get; init; }
        public string? CurrentPlayer { get; init; }

        public static SerializableBoard FromBoard(Board board)
        {
            return new()
            {
                Board = board.ToString(),
                CurrentPlayer = board.CurrentPlayer.ToString()
            };
        }

        public Board ToBoard()
        {
            if (Board == null)
                throw new InvalidOperationException("null board");
            if (CurrentPlayer == null)
                throw new InvalidOperationException("null current player");

            return Game.Board.CreateFromString(Board, Enum.Parse<BoardTile>(CurrentPlayer));
        }
    }
}
