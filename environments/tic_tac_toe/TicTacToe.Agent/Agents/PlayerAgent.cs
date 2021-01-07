using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class PlayerAgent : ITicTacToeAgent
    {
        public BoardTile Tile => _ticTacToePlayer.Tile;

        private readonly ITicTacToePlayer _ticTacToePlayer;

        public PlayerAgent(ITicTacToePlayer ticTacToePlayer)
        {
            _ticTacToePlayer = ticTacToePlayer;
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
            => _ticTacToePlayer.GetAction(environment.CurrentState);

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            throw new System.NotImplementedException();
        }

        public void SaveTrainedValues(string agentName, string path)
        {
            throw new System.NotImplementedException();
        }
    }
}