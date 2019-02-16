using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public interface ISearchProblem<TState, TAction>
    {
        TState InitialState { get; }

        /// <summary>
        /// Get available actions at the given state
        /// </summary>
        IEnumerable<TAction> GetActions(TState state);
        
        /// <summary>
        /// Do the given action and return the resultant state
        /// </summary>
        TState DoAction(TState state, TAction action);

        bool IsGoal(TState state);
        
        double PathCost(TState state, TAction action);
    }
}
