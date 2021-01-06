using System.Collections.Generic;

namespace RLCommon
{
    public interface IProblem<TState, TAction>
    {
        IEnumerable<TState> AllStates();
        IEnumerable<TAction> AvailableActions(TState state);

        /// <summary>
        /// Returns all possible states and their probability from the given state and action
        /// </summary>
        IEnumerable<(TState, double)> PossibleStates(TState state, TAction action);
    }
}