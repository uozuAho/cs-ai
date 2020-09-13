using System;

namespace dp.Examples.GamblersProblem
{
    class GamblersProblemExample
    {
        public static void Run()
        {
            UseDpToFindOptimalPolicy();
            // EvaluatePolicyValues();
            // PlayGamesWithPolicies();
        }

        private static void UseDpToFindOptimalPolicy()
        {
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 100;
            const int evaluationSweepsPerPolicyUpdate = 1;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(world);

            Console.WriteLine("Policy optimiser:");
            var (policy, values) = DpPolicyOptimiser
                .FindOptimalPolicy(world, rewarder, evaluationSweepsPerPolicyUpdate);
            Console.WriteLine("Optimal policy values:");
            PrintAllValues(world, values);
            Console.WriteLine("Optimal policy stakes:");
            PrintPolicyActions(world, policy);
        }

        private static void EvaluatePolicyValues()
        {
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 100;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(world);

            Console.WriteLine("Optimal policy:");
            var (policy, values) = DpPolicyOptimiser.FindOptimalPolicy(world, rewarder);
            var evalValues = new ValueTable<GamblersWorldState, GamblersWorldAction>(world);
            evalValues.Evaluate(policy, rewarder);
            PrintPolicyActions(world, policy);
            PrintAllValues(world, values);

            // Console.WriteLine("Always stake max policy:");
            // var policy = new AlwaysStakeMaxPolicy(world);
            // PrintPolicyActionsAndValues(world, policy, rewarder);
        }

        private static void PlayGamesWithPolicies()
        {
            var probabilityOfHeads = 0.2;
            int dollarsToWin = 100;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);

            // Console.WriteLine("random policy:");
            // PlayGamesWithPolicy(world, new UniformRandomGamblersPolicy());

            // Console.WriteLine("random policy:");
            // PlayGamesWithPolicy(world, new AlwaysStake1DollarPolicy());

            Console.WriteLine("always stake max policy:");
            PlayGamesWithPolicy(world, new AlwaysStakeMaxPolicy(world));

            // var (optimalPolicy, _) = FindOptimalPolicy(world, new GamblersWorldRewarder(world));
            // Console.WriteLine("optimal policy:");
            // PlayGamesWithPolicy(world, optimalPolicy);
        }

        private static void PlayGamesWithPolicy(GamblersWorld world, IGamblersPolicy policy)
        {
            var player = new GamblersProblemPlayer(world, policy);
            player.Play();
        }

        private static void PrintAllValues(
            GamblersWorld world,
            ValueTable<GamblersWorldState, GamblersWorldAction> values)
        {
            foreach (var state in world.AllStates())
            {
                Console.WriteLine($"{state}: {values.Value(state)}");
            }
        }

        private static void PrintPolicyActions(
            GamblersWorld world,
            IDeterministicPolicy<GamblersWorldState, GamblersWorldAction> policy)
        {
            foreach (var state in world.AllStates())
            {
                Console.WriteLine($"{state}: {policy.Action(state)}");
            }
        }
    }
}
