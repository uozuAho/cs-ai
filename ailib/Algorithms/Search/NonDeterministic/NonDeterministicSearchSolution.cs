using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicSearchSolution<TState, TAction>
    {
        private List<IPlanNode<TState, TAction>> _plan;
        private int _idx;

        public NonDeterministicSearchSolution(List<IPlanNode<TState, TAction>> plan)
        {
            _plan = plan;
            _idx = 0;
        }

        public TAction NextAction(TState state)
        {
            switch (_plan[_idx])
            {
                case OrNode<TState, TAction> orNode:
                    _idx++;
                    return orNode.Action;
                
                case AndNode<TState, TAction> andNode:
                    _plan = andNode.GetPlan(state).ToList();
                    var nextOrNode = _plan[0] as OrNode<TState, TAction>;
                    if (nextOrNode == null) throw new InvalidOperationException("This shouldn't happen");
                    _idx = 1;
                    return nextOrNode.Action;
                    
                default:
                    throw new InvalidOperationException("This shouldn't happen");
            }
        }
    }
}