using System;
using System.Linq;

namespace dp.GamblersProblem
{
    class GamblersProblemExample
    {
        public static void Run()
        {
            Test();
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

        private static void Test()
        {
            const double probabilityOfHeads = 0.4;

            for (var dollarsToWin = 5; dollarsToWin < 10; dollarsToWin++)
            {
                var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
                Assert($"world has {dollarsToWin + 1} states", world.AllStates().Count() == dollarsToWin + 1);
                Assert("0 possible actions when $0 in hand", !world.AvailableActions(new GamblersWorldState(0)).Any());
                Assert("4 possible actions when $3 in hand", world.AvailableActions(new GamblersWorldState(3)).Count() == 4);
                Assert("0 possible actions when $goal in hand", !world.AvailableActions(new GamblersWorldState(dollarsToWin)).Any());
            }

            var world2 = new GamblersWorld(probabilityOfHeads, 100);

            for (int i = 0; i < 100; i++)
            {
                var possibleStates = world2.PossibleStates(new GamblersWorldState(i), new GamblersWorldAction(1)).ToList();
                Assert("2 possible states on an action", possibleStates.Count == 2);
                Assert("probability of states sums to 1", possibleStates.Sum(s => s.Item2) == 1.0);
                Assert("probability of losing is 0.6", possibleStates.Single(s => s.Item1.DollarsInHand == i - 1).Item2 == 0.6);
            }
        }

        private static void Assert(string description, bool condition)
        {
            if (!condition)
            {
                Console.WriteLine("Failed: " + description);
            }
        }
    }
}
