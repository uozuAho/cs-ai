using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearchSolution<TState, TAction>
    {
        private IPlanNode<TState, TAction> _plan;

        private readonly Dictionary<TState, IPlanNode<TState, TAction>> _planHistory =
            new Dictionary<TState, IPlanNode<TState, TAction>>();

        public NonDeterministicDfsSearchSolution(IPlanNode<TState, TAction> plan)
        {
            _plan = plan;
        }

        public TAction NextAction(TState state)
        {
            if (_planHistory.ContainsKey(state))
            {
                _plan = _planHistory[state];
            }
            else
            {
                _planHistory[state] = _plan;
            }

            return AdvanceToNextOrNodeAndGetNextAction(state);
        }
        
        // todo: break this up
        private TAction AdvanceToNextOrNodeAndGetNextAction(TState state)
        {
            TAction action;
            
            switch (_plan)
            {
                case OrNode<TState, TAction> orNode:
                    action = orNode.Action;
                    _plan = orNode.Child;
                    break;
                
                case AndNode<TState, TAction> andNode:
                    _plan = andNode.GetPlan(state);
                    var nextOrNode = _plan as OrNode<TState, TAction>;
                    if (nextOrNode == null)
                        throw new InvalidOperationException("Next node after an and node should be an or node");
                    action = nextOrNode.Action;
                    _plan = nextOrNode.Child;
                    break;
                    
                default:
                    throw new InvalidOperationException("This shouldn't happen");
            }

            return action;
        }
    }
}