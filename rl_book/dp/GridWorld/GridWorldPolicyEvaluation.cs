using System;

namespace dp.GridWorld
{
    internal class GridWorldPolicyEvaluation
    {
        public static void Run()
        {
            var world = new GridWorld();
            var policy = new UniformRandomGridWorldPolicy();
            var rewarder = new NegativeAtNonTerminalStatesGridWorldRewarder();

            var gridValues = new GridWorldValueTable(world);
            var genericValues = new ValueTable<GridWorldState, GridWorldAction>(world);

            Console.WriteLine("values:");
            foreach (var state in world.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gridValue = gridValues.Value(state);

                Console.WriteLine($"state: {state}, generic: {genericValue}, grid: {gridValue}");
            }

            gridValues.Evaluate(policy, rewarder, 1);
            genericValues.Evaluate(policy, rewarder, 1);

            Console.WriteLine("values:");
            foreach (var state in world.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gridValue = gridValues.Value(state);

                Console.WriteLine($"state: {state}, generic: {genericValue}, grid: {gridValue}");
            }

            gridValues.Evaluate(policy, rewarder, 1);
            genericValues.Evaluate(policy, rewarder, 1);

            Console.WriteLine("values:");
            foreach (var state in world.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gridValue = gridValues.Value(state);

                Console.WriteLine($"state: {state}, generic: {genericValue}, grid: {gridValue}");
            }
        }
    }
}
