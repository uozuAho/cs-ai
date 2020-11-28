using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class MonteCarloTicTacToeAgent : IPlayer
    {
        public BoardTile Tile { get; }

        public MonteCarloTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
        }

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            return new TicTacToeAction {Position = 0, Tile = Tile};
        }
    }
}
