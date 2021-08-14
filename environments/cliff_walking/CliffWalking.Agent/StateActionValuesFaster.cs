namespace CliffWalking.Agent
{
    /// <summary>
    /// Cheats by knowing more about cliff walking - stores action values in
    /// an array instead of a dictionary.
    /// </summary>
    public class StateActionValuesFaster
    {
        private readonly double[,,] _values;

        public StateActionValuesFaster(int x, int y)
        {
            _values = new double[x,y,4];
        }

        public double Valuef(Position state, CliffWalkingAction action)
        {
            return _values[state.X, state.Y, (int) action];
        }

        public void SetValue(Position state, CliffWalkingAction action, double value)
        {
            _values[state.X, state.Y, (int) action] = value;
        }

        // public IEnumerable<(CliffWalkingAction, double)> ActionValues(Position position)
        // {
        //     if (_values.TryGetValue(position, out var actionValues))
        //         return actionValues.Select(av => (av.Key, av.Value));
        //     return Enumerable.Empty<(CliffWalkingAction, double)>();
        // }
    }
}