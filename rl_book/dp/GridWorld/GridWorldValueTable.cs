using System;
using System.Linq;

namespace dp.GridWorld
{
    public class GridWorldValueTable
    {
        private readonly GridWorld _world;
        private readonly double[] _values;

        public GridWorldValueTable(GridWorld world)
        {
            _world = world;
            _values = new double[world.AllStates().Count()];
        }

        public double Value(GridWorldState state)
        {
            return _values[state.Position1D];
        }

        public void Evaluate(IGridWorldPolicy policy, IGridWorldRewarder gridWorldRewarder)
        {
            var largestValueChange = 0.0;

            do
            {
                largestValueChange = 0.0;

                foreach (var state in _world.AllStates())
                {
                    var originalValue = Value(state);
                    var newValue = CalculateValue(state, policy, gridWorldRewarder);

                    _values[state.Position1D] = newValue;

                    var valueChange = Math.Abs(originalValue - newValue);
                    if (valueChange > largestValueChange) largestValueChange = valueChange;
                }

            } while (largestValueChange > 0.0001);
        }

        public void Print()
        {
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    var idx = y * 4 + x;
                    Console.Write("{0,6:0.00}", _values[idx]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private double CalculateValue(GridWorldState state, IGridWorldPolicy policy, IGridWorldRewarder gridWorldRewarder)
        {
            var newValue = 0.0;

            foreach (var action in _world.AvailableActions(state))
            {
                var nextState = _world.NextState(state, action);
                var reward = gridWorldRewarder.Reward(state, action);
                newValue += policy.PAction(state, action) * (reward + Value(nextState));
            }

            return newValue;
        }
    }
}