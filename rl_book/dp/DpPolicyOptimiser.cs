using System;
using System.Linq;

namespace dp
{
    class DpPolicyOptimiser
    {
        private const int EvaluationSweepsPerPolicyUpdate = 1;

        /// <summary>
        /// Uses value iteration to find the optimal policy and values for the given problem
        /// </summary>
        public static (IDeterministicPolicy<TState, TAction>, ValueTable<TState, TAction>)
            FindOptimalPolicy<TState, TAction>(
                IProblem<TState, TAction> problem,
                IRewarder<TState, TAction> rewarder)
        {
            const int maxIterations = 100;
            var values = new ValueTable<TState, TAction>(problem);
            IPolicy<TState, TAction> initialPolicy = new UniformRandomPolicy<TState, TAction>(problem);
            IDeterministicPolicy<TState, TAction> greedyPolicy = null;

            for (var i = 0; i < maxIterations; i++)
            {
                values.Evaluate(greedyPolicy ?? initialPolicy, rewarder, EvaluationSweepsPerPolicyUpdate);

                var newGreedyPolicy = GreedyPolicy<TState, TAction>.Create(problem, values, rewarder);

                if (newGreedyPolicy.HasSameActionsAs(greedyPolicy))
                {
                    Console.WriteLine($"Found optimal policy at iteration {i}");
                    break;
                }

                greedyPolicy = newGreedyPolicy;

                if (i == maxIterations - 1)
                {
                    Console.WriteLine($"Policy iteration did not converge by iteration {i}");
                }
            }

            return (greedyPolicy, values);
        }
    }

    internal class UniformRandomPolicy<TState, TAction> : IPolicy<TState, TAction>
    {
        private readonly IProblem<TState, TAction> _problem;

        public UniformRandomPolicy(IProblem<TState, TAction> problem)
        {
            _problem = problem;
        }

        public double PAction(TState state, TAction action)
        {
            var numActions = _problem.AvailableActions(state).Count();

            if (numActions == 0) return 0.0;

            return 1.0 / numActions;
        }
    }
}
