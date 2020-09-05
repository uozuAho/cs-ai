namespace dp.GridWorld
{
    internal class GridWorldPolicyEvaluation
    {
        public static void Run()
        {
            var world = new GridWorld();
            var policy = new UniformRandomGridWorldPolicy();
            var rewarder = new NegativeAtNonTerminalStatesGridWorldRewarder();

            var values = new GridWorldValueTable(world);

            values.Print();
            values.Evaluate(policy, rewarder);
            values.Print();
        }
    }
}
