using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class ConsoleInputPlayer : IPlayer
    {
        public BoardTile Tile { get; }

        public ConsoleInputPlayer(BoardTile playerTile)
        {
            Tile = playerTile;
        }

        public TicTacToeAction GetAction(IBoard board)
        {
            System.Console.WriteLine($"place {Tile} on tile (number):");
            var input = System.Console.ReadLine();
            var pos = int.Parse(input);

            return new TicTacToeAction
            {
                Position = pos,
                Tile = Tile
            };
        }
    }
}