using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToeMutablePolicy
    {
        private readonly BoardActionMap _actionMap = new();
        public int NumStates => _actionMap.NumStates;

        public TicTacToeAction Action(IBoard board)
        {
            return _actionMap.ActionFor(board);
        }

        public void SetAction(IBoard state, TicTacToeAction action)
        {
            _actionMap.SetAction(state, action);
        }

        public IPlayer ToPlayer(BoardTile tile)
        {
            return new TicTacToePolicyPlayer(tile, _actionMap);
        }

        public bool HasActionFor(IBoard board)
        {
            return _actionMap.HasActionFor(board);
        }
    }
}