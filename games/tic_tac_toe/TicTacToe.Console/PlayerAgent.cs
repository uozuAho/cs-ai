using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    internal class PlayerAgent : ITicTacToeAgent
    {
        public BoardTile Tile => _player.Tile;

        public TicTacToeAction GetAction(TicTacToeEnvironment environment, Board board) => _player.GetAction(board);

        private readonly IPlayer _player;

        public PlayerAgent(IPlayer player)
        {
            _player = player;
        }
    }
}