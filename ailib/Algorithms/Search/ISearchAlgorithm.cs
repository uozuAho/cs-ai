using System.Collections.Generic;

namespace ailib.Algorithms.Search
{
    public interface ISearchAlgorithm<TState, out TAction>
    {
        bool IsFinished { get; }
        
        bool IsSolved { get; }
        
        TState CurrentState { get; }
        
        int NumberOfExploredStates { get; }
        
        /// <summary>
        /// Get the actions from the initial state to the given state.
        /// Throws error if given state not explored 
        /// </summary>
        IEnumerable<TAction> GetSolutionTo(TState state);
        
        /// <summary>
        /// Get the actions required to reach the first goal found.
        /// </summary>
        IEnumerable<TAction> GetSolution();

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