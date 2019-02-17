using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    internal interface ISearchAlgorithm<TState, out TAction>
    {
        bool IsFinished { get; }
        
        TState CurrentState { get; }
        
        /// <summary>
        /// Get the actions from the initial state to the given state.
        /// Throws error if given state not explored 
        /// </summary>
        IEnumerable<TAction> GetSolutionTo(TState state);

        bool IsExplored(TState state);

        /// <summary>
        /// Perform one step of the search
        /// </summary>
        void Step();

        /// <summary>
        /// Run until solved or no solution
        /// </summary>
        void Solve();
    }
}