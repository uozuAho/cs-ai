using System.Collections.Generic;
using MoreLinq.Extensions;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    public class ActionValues
    {
        private readonly Dictionary<IBoard, Dictionary<TicTacToeAction, double>> _values = new();

        public void Set(IBoard state, TicTacToeAction action, double value)
        {
            if (_values.ContainsKey(state))
                _values[state][action] = value;
            else
                _values[state] = new Dictionary<TicTacToeAction, double> {{action, value}};
        }

        public TicTacToeAction HighestValueAction(IBoard state)
        {
            var actionValues = _values[state];

            return actionValues.MaxBy(av => av.Value).Single().Key;
        }
    }
}