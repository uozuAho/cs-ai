using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    /// <summary>
    /// I don't get how the book's 'and or search' is supposed to work.
    /// Here's my own. Still functional + recursive.
    /// </summary>
    public static class MyNonDetermDfsSearch<TState, TAction>
    {
        private static IPlanNode<TState, TAction> EmptyPlan { get; } =
            new EmptyPlan<TState, TAction>();
        
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
            
            foreach (var state in states)
            {
                var plan = OrSearch(state, problem, explored);

                // an and node's children must all lead to the goal
                if (plan == null) return null;
                
                thisPlan.AddPlanForState(state, plan);
            }

            return thisPlan;
        }
    }
    
    public class OrNode2<TState, TAction> : IPlanNode<TState, TAction>
    {
        public TAction Action { get; }
        public IPlanNode<TState, TAction> Child { get; set; }

        public OrNode2(TAction action)
        {
            Action = action;
        }
    }

    public class MyNonDeterministicDfsSearchSolution<TState, TAction> : INonDeterministicSearchSolution<TState, TAction>
    {
        private IPlanNode<TState, TAction> _currentNode;

        public MyNonDeterministicDfsSearchSolution(IPlanNode<TState, TAction> root)
        {
            _currentNode = root;
        }
        
        public TAction NextAction(TState state)
        {
            while (_currentNode is AndNode<TState, TAction> andNode)
            {
                _currentNode = andNode.GetPlan(state);
            }

            if (!(_currentNode is OrNode2<TState, TAction> orNode))
                throw new InvalidOperationException("current node should be 'or node'");
            
            var action = orNode.Action;
            _currentNode = orNode.Child;
            
            return action;
        }
    }
}