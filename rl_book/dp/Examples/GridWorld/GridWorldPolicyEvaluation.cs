namespace dp.Examples.GridWorld
{
    internal class GridWorldPolicyEvaluation
    {
        public static void Run()
        {
            var world = new GridWorld();
            var policy = new UniformRandomGridWorldPolicy();
            var rewarder = new NegativeAtNonTerminalStatesGridWorldRewarder();

            var gridValues = new GridWorldValueTable(world);

            gridValues.Evaluate(policy, rewarder);
            gridValues.Print();
        }
    }
}
