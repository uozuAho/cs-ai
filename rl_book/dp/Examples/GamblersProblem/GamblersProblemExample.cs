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
            const int dollarsToWin = 10;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            // Console.WriteLine("Random policy");
            IGamblersPolicy policy = new UniformRandomGamblersPolicy();
            // EvaluatePolicy(world, policy);

            Console.WriteLine("Always $1 policy");
            policy = new AlwaysStake1DollarPolicy();
            EvaluatePolicy(world, policy);

            // policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // Console.WriteLine("Greedy policy:");
            // ((GreedyGamblersPolicy) policy)?.Print();
            //
            // values.Evaluate(policy, rewarder);
            // Console.WriteLine("Values:");
            // values.Print();
            //
            // policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // Console.WriteLine("Greedy policy:");
            // ((GreedyGamblersPolicy) policy)?.Print();

            // for (var i = 0; i < 100; i++)
            // {
            //     // todo: this currently evaluates with many iterations. change to value iteration,
            //     // see how it affects convergence rate
            //     values.Evaluate(policy, rewarder);
            //     policy = GreedyGamblersPolicy.Create(world, values, rewarder);
            // }
            //
            // Console.WriteLine("Policy:");
            // (policy as GreedyGamblersPolicy)?.Print();
            // Console.WriteLine();
            // Console.WriteLine("Values:");
            // values.Print();
        }

        private static void EvaluatePolicy(GamblersWorld world, IGamblersPolicy policy)
        {
            var rewarder = new GamblersWorldRewarder(world);
            var values = new GamblersValueTable(world);

            values.Evaluate(policy, rewarder);
            Console.WriteLine("Values:");
            values.Print();
            // values.Evaluate(policy, rewarder, 1);
            // Console.WriteLine("Values:");
            // values.Print();
            // values.Evaluate(policy, rewarder, 1);
            // Console.WriteLine("Values:");
            // values.Print();
        }
    }
}
