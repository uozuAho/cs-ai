using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class ActionValueTable
    {
        private readonly Dictionary<Board, Dictionary<TicTacToeAction, double>> _values = new();

        public void Set(Board state, TicTacToeAction action, double value)
        {
            if (_values.ContainsKey(state))
                _values[state][action] = value;
            else
                _values[state] = new Dictionary<TicTacToeAction, double> {{action, value}};
        }

        public IEnumerable<(Board, TicTacToeAction)> HighestValueActions()
        {
            return _values.Select(kv => (kv.Key, HighestValueAction(kv.Key)));
        }

        public TicTacToeAction HighestValueAction(Board state)
        {
            var actionValues = _values[state];

            return actionValues.MaxBy(av => av.Value).First().Key;
        }

        public double HighestValue(Board state)
        {
            return _values[state].Values.Max();
        }
    }
}