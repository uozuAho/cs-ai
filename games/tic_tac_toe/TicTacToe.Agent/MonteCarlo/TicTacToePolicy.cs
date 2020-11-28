using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class TicTacToePolicy
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new Dictionary<string, TicTacToeAction>();

        public IEnumerable<string> States => _actionMap.Keys.Select(k => k);

        public TicTacToeAction Action(string state)
        {
            return _actionMap[state];
        }

        public void AddAction(IBoard state, TicTacToeAction action)
        {
            _actionMap[state.AsString()] = action;
        }
    }
}