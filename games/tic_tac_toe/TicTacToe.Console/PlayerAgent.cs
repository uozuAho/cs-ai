using TicTacToe.Agent;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    internal class PlayerAgent : ITicTacToeAgent
    {
        public BoardTile Tile => _ticTacToePlayer.Tile;

        public TicTacToeAction GetAction(TicTacToeEnvironment environment, Board board) => _ticTacToePlayer.GetAction(board);

        private readonly ITicTacToePlayer _ticTacToePlayer;

        public PlayerAgent(ITicTacToePlayer ticTacToePlayer)
        {
            _ticTacToePlayer = ticTacToePlayer;
        }
    }
}