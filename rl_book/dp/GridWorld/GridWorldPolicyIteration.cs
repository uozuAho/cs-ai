namespace dp.GridWorld
{
    class GridWorldPolicyIteration
    {
        public static void Run()
        {
            var world = new GridWorld();
            var randomPolicy = new UniformRandomGridWorldPolicy();
            var rewarder = new NegativeAtNonTerminalStatesGridWorldRewarder();

            var values = new GridWorldValueTable(world);

            // manually iterate a couple of times - optimal policy is greedy wrt
            // initial random policy values

            values.Evaluate(randomPolicy, rewarder);
            values.Print();

            var greedyPolicy = GreedyGridWorldPolicy.Create(world, values, rewarder);

            values.Evaluate(greedyPolicy, rewarder);
            values.Print();

            greedyPolicy = GreedyGridWorldPolicy.Create(world, values, rewarder);

            values.Evaluate(greedyPolicy, rewarder);
            values.Print();

            greedyPolicy.Print();
        }
    }
}
