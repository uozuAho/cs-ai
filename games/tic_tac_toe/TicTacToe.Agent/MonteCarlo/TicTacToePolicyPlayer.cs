using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToePolicyPlayer : IPlayer
    {
        public BoardTile Tile { get; }
        private readonly BoardActionMap _actionMap;

        public TicTacToePolicyPlayer(BoardTile tile, BoardActionMap actionMap)
        {
            _actionMap = actionMap;
            Tile = tile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _actionMap.ActionFor(board);
        }
    }
}