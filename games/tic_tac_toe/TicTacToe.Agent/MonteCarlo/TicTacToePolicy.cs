using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToePolicy
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new();

        public IEnumerable<string> States => _actionMap.Keys.Select(k => k);

        public TicTacToeAction Action(string state)
        {
            return _actionMap[state];
        }

        public void SetAction(IBoard state, TicTacToeAction action)
        {
            _actionMap[state.AsString()] = action;
        }
    }
}