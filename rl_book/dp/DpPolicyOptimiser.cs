using System.Linq;

namespace dp
{
    class DpPolicyOptimiser
    {
        private const int EvaluationSweepsPerPolicyUpdate = 5;

        /// <summary>
        /// Uses value iteration to find the optimal policy and values for the given problem
        /// </summary>
        public static (IDeterminatePolicy<TState, TAction>, ValueTable<TState, TAction>)
            FindOptimalPolicy<TState, TAction>(
                IProblem<TState, TAction> problem,
                IRewarder<TState, TAction> rewarder)
        {
            var values = new ValueTable<TState, TAction>(problem);
            IPolicy<TState, TAction> policy = new UniformRandomPolicy<TState, TAction>(problem);

            // todo: iterate until greedy policy doesn't change (?)
            for (var i = 0; i < 100; i++)
            {
                values.Evaluate(policy, rewarder, EvaluationSweepsPerPolicyUpdate);
                policy = GreedyPolicy<TState, TAction>.Create(problem, values, rewarder);
            }

            return (policy as IDeterminatePolicy<TState, TAction>, values);
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
