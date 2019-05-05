using System;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearchSolution<TState, TAction> : INonDeterministicSearchSolution<TState, TAction>
    {
        private IPlanNode<TState, TAction> _currentNode;

        public NonDeterministicDfsSearchSolution(IPlanNode<TState, TAction> root)
        {
            _currentNode = root;
        }
        
        public TAction NextAction(TState state)
        {
            while (_currentNode is AndNode<TState, TAction> andNode)
            {
                _currentNode = andNode.GetPlan(state);
            }

            if (!(_currentNode is OrNode<TState, TAction> orNode))
                throw new InvalidOperationException("current node should be 'or node'");
            
            var action = orNode.Action;
            _currentNode = orNode.Child;
            
            return action;
        }
    }
}