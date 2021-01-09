using System.Collections.Generic;
using System.Linq;

namespace CliffWalking.Agent
{
    public class StateActionValues
    {
        private const double DefaultActionValue = 0;
        private readonly Dictionary<Position, Dictionary<CliffWalkingAction, double>> _values = new();

        public double Value(Position state, CliffWalkingAction action)
        {
            if (_values.TryGetValue(state, out var actionValues))
            {
                if (actionValues.TryGetValue(action, out var value))
                {
                    return value;
                }
            }

            return DefaultActionValue;
        }

        public void SetValue(Position state, CliffWalkingAction action, double value)
        {
            if (!_values.ContainsKey(state))
            {
                _values[state] = new Dictionary<CliffWalkingAction, double>
                {
                    {action, value}
                };
            }
            else
            {
                _values[state][action] = value;
            }
        }

        public IEnumerable<(CliffWalkingAction, double)> ActionValues(Position position)
        {
            if (_values.TryGetValue(position, out var actionValues))
                return actionValues.Select(av => (av.Key, av.Value));
            return Enumerable.Empty<(CliffWalkingAction, double)>();
        }
    }
}