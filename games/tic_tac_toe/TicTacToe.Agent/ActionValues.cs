using System.Collections.Generic;
using MoreLinq.Extensions;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class ActionValues
    {
        private readonly Dictionary<Board, Dictionary<TicTacToeAction, double>> _values = new();

        public void Set(Board state, TicTacToeAction action, double value)
        {
            if (_values.ContainsKey(state))
                _values[state][action] = value;
            else
                _values[state] = new Dictionary<TicTacToeAction, double> {{action, value}};
        }

        public TicTacToeAction HighestValueAction(Board state)
        {
            var actionValues = _values[state];

            return actionValues.MaxBy(av => av.Value).First().Key;
        }
    }
}