using System;

namespace dp.Examples.GamblersProblem
{
    class GamblersProblemExample
    {
        public static void Run()
        {
            RunImpl();
        }

        private static void RunImpl()
        {
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 100;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            // Console.WriteLine("Random policy");
            // IGamblersPolicy policy = new UniformRandomGamblersPolicy();
            // EvaluatePolicy(world, policy);
            //
            // Console.WriteLine("Always $1 policy");
            // policy = new AlwaysStake1DollarPolicy();
            // EvaluatePolicy(world, policy);
            //
            // policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // Console.WriteLine("Greedy policy built from always $1 policy:");
            // EvaluatePolicy(world, policy);
            // Console.WriteLine("Greedy policy stakes:");
            // ((GreedyGamblersPolicy) policy)?.Print();

            var (optimalPolicy, optimalValues) = FindOptimalPolicy(world, rewarder);
            Console.WriteLine("Optimal policy values:");
            optimalValues.Print();
            Console.WriteLine("Optimal policy stakes:");
            ((GreedyGamblersPolicy)optimalPolicy)?.Print();
        }

        private static (IGamblersPolicy, GamblersValueTable) FindOptimalPolicy(
            GamblersWorld world, GamblersWorldRewarder rewarder)
        {
            const int sweepsPerPolicyUpdate = 50;
            var values = new GamblersValueTable(world);
            IGamblersPolicy policy = new UniformRandomGamblersPolicy();

            // todo: iterate until greedy policy doesn't change (?)
            for (var i = 0; i < 100; i++)
            {
                values.Evaluate(policy, rewarder, sweepsPerPolicyUpdate);
                // Console.WriteLine("values:");
                // values.Print();
                policy = GreedyGamblersPolicy.Create(world, values, rewarder);
                // Console.WriteLine("greedy stakes:");
                // ((GreedyGamblersPolicy)policy)?.Print();
            }

            return (policy, values);
        }

        private static void EvaluatePolicy(GamblersWorld world, IGamblersPolicy policy)
        {
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            values.Evaluate(policy, rewarder);
            Console.WriteLine("Values:");
            values.Print();
        }
    }
}
