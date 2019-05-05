using System;
using System.Collections.Generic;

namespace ailib.Algorithms.Search.NonDeterministic
{
    internal class AndNode<TState, TAction> : IPlanNode<TState, TAction>
    {
        private readonly Dictionary<TState, IPlanNode<TState, TAction>> _children =
            new Dictionary<TState, IPlanNode<TState, TAction>>();

        public void AddPlanForState(TState state, IPlanNode<TState, TAction> planNode)
        {
            if (_children.ContainsKey(state)) throw new InvalidOperationException("duplicate state");
            
            _children[state] = planNode;
        }

        public IPlanNode<TState, TAction> GetPlan(TState state)
        {
            return _children[state];
        }
    }
}