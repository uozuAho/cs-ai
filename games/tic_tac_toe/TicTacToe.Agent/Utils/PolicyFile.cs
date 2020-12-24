using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record PolicyFile(
        string Name,
        string Description,
        BoardTile Tile,
        PolicyFileAction[] Actions)
    {
        public static PolicyFile FromBoardActionMap(string name, string description, BoardTile boardTile, BoardActionMap map)
        {
            var policyActions = map.AllActions().Select(a => new PolicyFileAction(
                a.Item1.ToString(), 0, a.Item2.Position)).ToArray();

            return new PolicyFile(name, description, boardTile, policyActions);
        }
    }
}