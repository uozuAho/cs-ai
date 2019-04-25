using System;
using System.Collections.Generic;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicSearchSolution<TState, TAction>
    {
        private readonly Dictionary<TState, TAction> _actionMap = new Dictionary<TState, TAction>();

        public TAction NextAction(TState state)
        {
            return _actionMap[state];
        }

        public void AddAction(TState state, TAction action)
        {
            if (_actionMap.ContainsKey(state)) throw new InvalidOperationException("state already exists");

            _actionMap[state] = action;
        }

        public bool Contains(TState state)
        {
            return _actionMap.ContainsKey(state);
        }
    }
}