using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record StateValuePolicy(
        string Name,
        string Description,
        BoardTile Tile,
        StateValueTable StateValueTable) : ITicTacToePolicy
    {
        public ITicTacToePlayer ToPlayer()
        {
            return new GreedyStateValuePlayer(StateValueTable, Tile);
        }
    }
}