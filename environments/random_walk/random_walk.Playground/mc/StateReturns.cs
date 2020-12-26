using System.Collections.Generic;
using System.Linq;

namespace random_walk.Playground.mc
{
    internal class StateReturns
    {
        private readonly List<double>[] _returns;

        public StateReturns(int numStates)
        {
            _returns = Enumerable.Range(0, numStates)
                .Select(_ => new List<double>()).ToArray();
        }

        public void Add(int state, double value)
        {
            _returns[state].Add(value);
        }

        public double AverageReturnFrom(int state)
        {
            return _returns[state].Average();
        }
    }
}