using TicTacToe.Agent;
using TicTacToe.Agent.Environment;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    internal class PlayerAgent : ITicTacToeAgent
    {
        public BoardTile Tile => _ticTacToePlayer.Tile;

        public TicTacToeAction GetAction(TicTacToeEnvironment environment) => _ticTacToePlayer.GetAction(environment.CurrentState);

        private readonly ITicTacToePlayer _ticTacToePlayer;

        public PlayerAgent(ITicTacToePlayer ticTacToePlayer)
        {
            _ticTacToePlayer = ticTacToePlayer;
        }
    }
}