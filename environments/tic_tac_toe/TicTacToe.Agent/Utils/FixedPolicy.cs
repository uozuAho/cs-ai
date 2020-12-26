using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class FixedPolicy
    {
        private readonly Dictionary<Board, TicTacToeAction> _actionMap = new();
        public int NumStates => _actionMap.Count;

        public TicTacToeAction Action(Board board)
        {
            return _actionMap[board];
        }

        public void SetAction(Board board, TicTacToeAction action)
        {
            _actionMap[board] = action;
        }

        public bool HasActionFor(Board board)
        {
            return _actionMap.ContainsKey(board);
        }

        public IEnumerable<(Board, TicTacToeAction)> AllActions()
        {
            return _actionMap.Select(pair => (pair.Key, pair.Value));
        }
    }
}