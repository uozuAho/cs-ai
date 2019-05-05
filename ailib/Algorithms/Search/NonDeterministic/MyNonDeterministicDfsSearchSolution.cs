using System;

namespace ailib.Algorithms.Search.NonDeterministic
{
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