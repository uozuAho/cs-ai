using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class BoardActionMap
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new();

        public TicTacToeAction ActionFor(IBoard board)
        {
            return _actionMap[board.AsString()];
        }

        public void SetAction(IBoard state, TicTacToeAction action)
        {
            _actionMap[state.AsString()] = action;
        }
    }
}