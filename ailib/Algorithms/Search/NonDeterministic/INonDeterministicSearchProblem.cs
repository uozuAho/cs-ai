using System.Collections.Generic;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface INonDeterministicSearchProblem<TState, TAction>
    {
        bool IsGoal(TState state);
        
        /// <summary>
        /// Get available actions at the given state
        /// </summary>
        IEnumerable<TAction> GetActions(TState state);
        
        /// <summary>
        /// Do the given action and return the resultant possible states
        /// </summary>
        IEnumerable<TState> DoAction(TState state, TAction action);
    }
}