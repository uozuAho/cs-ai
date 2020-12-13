using System;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class ConsoleInputTicTacToePlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        public ConsoleInputTicTacToePlayer(BoardTile playerTile)
        {
            Tile = playerTile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            System.Console.WriteLine($"place {Tile} on tile (number):");
            var input = System.Console.ReadLine();

            if (input == null) throw new InvalidOperationException("no!");

            var pos = int.Parse(input);

            return new TicTacToeAction
            {
                Position = pos,
                Tile = Tile
            };
        }
    }
}