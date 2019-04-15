using System;
using System.Collections.Generic;
using System.Linq;
using ailib.Algorithms.Search;

namespace vacuum_world
{
    public class VacuumWorldSearchProblem : ISearchProblem<VacuumWorldState, VacuumWorldAction>
    {
        public VacuumWorldState InitialState { get; }
        
        private readonly IVacuumWorldActionHandler _actionHandler;
        
        public VacuumWorldSearchProblem(VacuumWorldState initialState, IVacuumWorldActionHandler actionHandler)
        {
            InitialState = initialState;
            _actionHandler = actionHandler;
        }
        
        public IEnumerable<VacuumWorldAction> GetActions(VacuumWorldState state)
        {
            return Enum.GetValues(typeof(VacuumWorldAction)).Cast<VacuumWorldAction>();
        }

        public VacuumWorldState DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            var newState = state.Clone();
            _actionHandler.DoAction(newState, action);
            return newState;
        }

        public bool IsGoal(VacuumWorldState state)
        {
            return AllSquaresAreClean(state);
        }

        public double PathCost(VacuumWorldState state, VacuumWorldAction action)
        {
            return 1;
        }

        private static bool AllSquaresAreClean(VacuumWorldState state)
        {
            for (var i = 0; i < state.WorldSize; i++)
            {
                for (var j = 0; j < state.WorldSize; j++)
                {
                    if (state.SquareIsDirty(i, j)) return false;
                }
            }

            return true;
        }
    }
}
