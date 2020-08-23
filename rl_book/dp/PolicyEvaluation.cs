using System;
using System.Collections.Generic;
using System.Linq;

namespace dp
{
    internal class PolicyEvaluation
    {
        public static void Run()
        {
            var world = new GridWorld();
            var policy = new UniformRandomPolicy();
            var rewarder = new NegativeAtNonTerminalStatesRewarder();

            var values = new ValueTable(world);

            values.Print();
            values.Evaluate(policy, rewarder);
            values.Print();
        }
    }

    /// <summary>
    /// Grid world problem of the DP chapter of the RL book. 4x4 grid, where 0,0
    /// and 3,3 are the terminal states.
    /// </summary>
    internal class GridWorld
    {
        public enum Action
        {
            Up,
            Down,
            Left,
            Right
        }

        public IEnumerable<Action> AvailableActions(GridWorldState state)
        {
            return (Action[])Enum.GetValues(typeof(Action));
        }

        public IEnumerable<GridWorldState> AllStates()
        {
            for (var i = 0; i < 16; i++)
            {
                yield return new GridWorldState(i);
            }
        }

        public GridWorldState NextState(GridWorldState state, Action action)
        {
            if (state.IsTerminal) return state;

            var x = state.Position1D % 4;
            var y = state.Position1D / 4;

            var newX = x;
            var newY = y;

            switch (action)
            {
                case Action.Up:
                    if (y > 0) newY--;
                    break;
                case Action.Down:
                    if (y < 3) newY++;
                    break;
                case Action.Left:
                    if (x > 0) newX--;
                    break;
                case Action.Right:
                    if (x < 3) newX++;
                    break;
            }

            return new GridWorldState(newY * 4 + newX);
        }
    }

    internal readonly struct GridWorldState
    {
        /// <summary>
        /// Position of the agent in the grid world. 0 is 0,0, 15 is 3,3
        /// </summary>
        public int Position1D { get; }

        public bool IsTerminal => Position1D == 0 || Position1D == 15;

        public GridWorldState(int position1D)
        {
            Position1D = position1D;
        }
    }

    internal interface IGridWorldPolicy
    {
        /// <summary>
        /// probability of the action from the given state
        /// </summary>
        double PAction(GridWorldState state, GridWorld.Action action);
    }

    internal class UniformRandomPolicy : IGridWorldPolicy
    {
        public double PAction(GridWorldState state, GridWorld.Action action)
        {
            return 0.25;
        }
    }

    interface IRewarder
    {
        double Reward(GridWorldState state, GridWorld.Action action);
    }

    internal class NegativeAtNonTerminalStatesRewarder : IRewarder
    {
        public double Reward(GridWorldState state, GridWorld.Action action)
        {
            if (state.IsTerminal) return 0;

            return -1;
        }
    }

    internal class ValueTable
    {
        private readonly GridWorld _world;
        private readonly double[] _values;

        public ValueTable(GridWorld world)
        {
            _world = world;
            _values = new double[world.AllStates().Count()];
        }

        public double Value(GridWorldState state)
        {
            return _values[state.Position1D];
        }

        public void Evaluate(IGridWorldPolicy policy, IRewarder rewarder)
        {
            var largestValueChange = 0.0;

            do
            {
                largestValueChange = 0.0;

                foreach (var state in _world.AllStates())
                {
                    var originalValue = Value(state);
                    var newValue = CalculateValue(state, policy, rewarder);

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

        private double CalculateValue(GridWorldState state, IGridWorldPolicy policy, IRewarder rewarder)
        {
            var newValue = 0.0;

            foreach (var action in _world.AvailableActions(state))
            {
                var nextState = _world.NextState(state, action);
                var reward = rewarder.Reward(state, action);
                newValue += policy.PAction(state, action) * (reward + Value(nextState));
            }

            return newValue;
        }
    }
}
