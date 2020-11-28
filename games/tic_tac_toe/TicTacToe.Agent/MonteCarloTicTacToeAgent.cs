using System.Linq;
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
            return game.GetAvailableActions().First();
        }
    }
}
