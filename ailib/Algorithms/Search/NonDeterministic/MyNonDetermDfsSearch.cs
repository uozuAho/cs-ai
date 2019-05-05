using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    /// <summary>
    /// Solves non deterministic search problems using depth first search.
    /// Inspired by 'and or graph search' translated from pg 136 of the ai book.
    /// Handling of (un)avoidable dead ends is not tested ... use at your own risk :)
    /// </summary>
    public class MyNonDetermDfsSearch<TState, TAction> : INonDeterministicSearchAlgorithm<TState, TAction>
    {
        private readonly INonDeterministicSearchProblem<TState, TAction> _problem;
        private readonly TState _initialState;

        private static IPlanNode<TState, TAction> EmptyPlan { get; } =
            new EmptyPlanNode<TState, TAction>();
        
        public MyNonDetermDfsSearch(INonDeterministicSearchProblem<TState, TAction> problem, TState initialState)
        {
            _problem = problem;
            _initialState = initialState;
        }
        
        public INonDeterministicSearchSolution<TState, TAction> GetSolution()
        {
            return Search(_problem, _initialState);
        }
        
        public static INonDeterministicSearchSolution<TState, TAction> Search(
            INonDeterministicSearchProblem<TState, TAction> problem,
            TState initialState)
        {
            var plan = OrSearch(initialState, problem, new Dictionary<TState, IPlanNode<TState, TAction>>());

            return new MyNonDeterministicDfsSearchSolution<TState, TAction>(plan);
        }

        // returns the first action that yields a non-null plan, else null
        private static IPlanNode<TState,TAction> OrSearch(
            TState state,
            INonDeterministicSearchProblem<TState,TAction> problem,
            IReadOnlyDictionary<TState, IPlanNode<TState, TAction>> explored)
        {
            if (problem.IsGoal(state)) return EmptyPlan;
            if (explored.ContainsKey(state)) return explored[state];

            foreach (var action in problem.GetActions(state))
            {
                var orNode = new OrNode2<TState, TAction>(action);
                var exploredCopy = explored.ToDictionary(e => e.Key, e => e.Value);
                exploredCopy[state] = orNode; 
                var plan = AndSearch(problem.DoAction(state, action), problem, exploredCopy);
                if (plan == null) continue;
                orNode.Child = plan;
                return orNode;
            }

            return null;
        }

        // returns a plan for each of the given states, if all plans are not null, else null
        private static IPlanNode<TState, TAction> AndSearch(
            IEnumerable<TState> states,
            INonDeterministicSearchProblem<TState,TAction> problem,
            IReadOnlyDictionary<TState, IPlanNode<TState, TAction>> explored)
        {
            var thisPlan = new AndNode<TState, TAction>();
            var statesList = states.ToList();
            
            // all child states have already been explored - path to goal is not this way (?)
            if (statesList.All(explored.ContainsKey)) return null;
            
            foreach (var state in statesList)
            {
                var plan = OrSearch(state, problem, explored);

                // an and node's children must all lead to the goal
                if (plan == null) return null;
                
                thisPlan.AddPlanForState(state, plan);
            }

            return thisPlan;
        }
    }
}