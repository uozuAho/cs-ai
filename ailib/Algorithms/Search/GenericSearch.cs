using System;
using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public abstract class GenericSearch<TState, TAction> : ISearchAlgorithm<TState, TAction>
    {
        public bool IsFinished { get; private set; }
        public bool IsSolved { get; private set; }
        public TState CurrentState { get; private set; }

        protected ISearchFrontier<SearchNode<TState, TAction>> _frontier;
        
        private ISearchProblem<TState, TAction> _problem;
        private readonly Dictionary<TState, SearchNode<TState, TAction>> _explored;
        private SearchNode<TState, TAction> _goal;

        protected GenericSearch(ISearchProblem<TState, TAction> problem)
        {
            _problem = problem;
            CurrentState = problem.InitialState;
            _explored = new Dictionary<TState, SearchNode<TState, TAction>>();

            IsFinished = false;
        }

        public IEnumerable<TAction> GetSolutionTo(TState state)
        {
            if (!_explored.ContainsKey(state)) throw new ArgumentException("cannot get solution to unexplored state");

            var actions = new List<TAction>();
            var currentNode = _explored[state];
            while (currentNode != null && currentNode.Action != null)
            {
                actions.Add(currentNode.Action);
                currentNode = currentNode.Parent;
            }

            actions.Reverse();
            return actions;
        }

        public bool IsExplored(TState state)
        {
            return _explored.ContainsKey(state);
        }

        public void Step()
        {
            if (IsFinished) return;
            if (_frontier.IsEmpty())
            {
                IsFinished = true;
                return;
            }

            var node = _frontier.Pop();
            while (node != null  && _explored.ContainsKey(node.State))
            {
                node = _frontier.Pop();
            }

            _explored[node.State] = node;
            CurrentState = node.State;
            var actions = _problem.GetActions(node.State);
            foreach (var action in actions)
            {
                var childState = _problem.DoAction(node.State, action);
                var childCost = node.PathCost + _problem.PathCost(node.State, action);
                var child = new SearchNode<TState, TAction>(childState, node, action, childCost);
                if (!_explored.ContainsKey(childState) && !_frontier.Contains(child)) {
                    if (_problem.IsGoal(childState)) {
                        _goal = child;
                        CurrentState = childState;
                        IsFinished = true;
                        IsSolved = true;
                        // add goal to explored to allow this.getSolutionTo(goal)
                        _explored[childState] = child;
                    }
                    else
                    {
                        _frontier.Push(child);
                    }
                }
            }
        }

        public void Solve()
        {
            while (!IsFinished)
            {
                Step();
            }
        }
    }
}