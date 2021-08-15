using System.Collections.Generic;

namespace CliffWalking.Agent
{
    /// <summary>
    /// Cheats by knowing more about cliff walking - stores action values in
    /// an array instead of a dictionary.
    /// </summary>
    public class StateActionValuesFaster : IStateActionValues
    {
        private readonly double[,,] _values;

        public StateActionValuesFaster(int x, int y)
        {
            _values = new double[x,y,4];
        }

        public double Value(Position state, CliffWalkingAction action)
        {
            return _values[state.X, state.Y, (int) action];
        }

        public void SetValue(Position state, CliffWalkingAction action, double value)
        {
            _values[state.X, state.Y, (int) action] = value;
        }

        public IEnumerable<(CliffWalkingAction, double)> ActionValues(Position position)
        {
            for (var i = 0; i < 4; i++)
            {
                yield return ((CliffWalkingAction) i, _values[position.X, position.Y, i]);
            }
        }
    }
}