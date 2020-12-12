using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class BoardActionMap
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new();
        public int NumStates => _actionMap.Count;

        public TicTacToeAction ActionFor(Board board)
        {
            return _actionMap[board.ToString()];
        }

        public void SetAction(Board state, TicTacToeAction action)
        {
            _actionMap[state.ToString()] = action;
        }

        public bool HasActionFor(Board board)
        {
            return _actionMap.ContainsKey(board.ToString());
        }

        public IEnumerable<(Board, TicTacToeAction)> AllActions()
        {
            return _actionMap.Select(action => (Board.CreateFromString(action.Key), action.Value));
        }
    }
}