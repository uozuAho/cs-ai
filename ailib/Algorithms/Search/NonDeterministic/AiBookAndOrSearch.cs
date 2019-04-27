using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    /// <summary>
    /// 'and or graph search' translated from pg 136 of the ai book
    /// </summary>
    public static class AiBookAndOrSearch<TState, TAction>
    {
        // note: failure = null

        private static IEnumerable<IPlanNode<TState, TAction>> EmptyPlan { get; } =
            Enumerable.Empty<IPlanNode<TState, TAction>>();

        public static IEnumerable<IPlanNode<TState, TAction>> AndOrGraphSearch(
            INonDeterministicSearchProblem<TState, TAction> problem,
            TState initialState)
        {
            return OrSearch(initialState, problem, new List<TState>());
        }

        private static IEnumerable<IPlanNode<TState, TAction>> OrSearch(
            TState state,
            INonDeterministicSearchProblem<TState,TAction> problem,
            ICollection<TState> path)
        {
            if (problem.IsGoal(state)) return EmptyPlan;
            if (path.Contains(state)) return null;

            foreach (var action in problem.GetActions(state))
            {
                var plan = AndSearch(problem.DoAction(state, action), problem, path.Concat(new[] {state}));
                if (plan != null)
                    return new[] {new OrNode<TState, TAction>(action), plan};
            }

            return null;
        }

        private static IPlanNode<TState, TAction> AndSearch(
            IEnumerable<TState> states,
            INonDeterministicSearchProblem<TState,TAction> problem,
            IEnumerable<TState> path)
        {
            var pathList = path.ToList();
            var thisPlan = new AndNode<TState, TAction>();
            
            foreach (var state in states)
            {
                var plan = OrSearch(state, problem, pathList);
                
                if (plan == null) return null;
                
                thisPlan.AddPlanForState(state, plan);
            }

            return thisPlan;
        }
    }

    public interface IPlanNode<TState, TAction>
    {
    }

    public class OrNode<TState, TAction> : IPlanNode<TState, TAction>
    {
        public TAction Action { get; }

        public OrNode(TAction action)
        {
            Action = action;
        }
    }
    
    public class AndNode<TState, TAction> : IPlanNode<TState, TAction>
    {
        private readonly Dictionary<TState, IEnumerable<IPlanNode<TState, TAction>>> _statePlans =
            new Dictionary<TState, IEnumerable<IPlanNode<TState, TAction>>>();

        public void AddPlanForState(TState state, IEnumerable<IPlanNode<TState, TAction>> planNode)
        {
            if (_statePlans.ContainsKey(state)) throw new InvalidOperationException("duplicate state");
            
            _statePlans[state] = planNode;
        }

        public IEnumerable<IPlanNode<TState, TAction>> GetPlan(TState state)
        {
            return _statePlans[state];
        }
    }
}