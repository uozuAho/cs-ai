using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class BoardActionMap
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new();
        public int NumStates => _actionMap.Count;

        public TicTacToeAction ActionFor(IBoard board)
        {
            return _actionMap[board.ToString()];
        }

        public void SetAction(IBoard state, TicTacToeAction action)
        {
            _actionMap[state.ToString()] = action;
        }

        public bool HasActionFor(IBoard board)
        {
            return _actionMap.ContainsKey(board.ToString());
        }
    }
}