using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToeMutablePolicy
    {
        private readonly BoardActionMap _actionMap = new();
        public int NumStates => _actionMap.NumStates;

        public TicTacToeAction Action(Board board)
        {
            return _actionMap.ActionFor(board);
        }

        public void SetAction(Board state, TicTacToeAction action)
        {
            _actionMap.SetAction(state, action);
        }

        public IPlayer ToPlayer(BoardTile tile)
        {
            return new TicTacToePolicyPlayer(tile, _actionMap);
        }

        public bool HasActionFor(Board board)
        {
            return _actionMap.HasActionFor(board);
        }
    }
}