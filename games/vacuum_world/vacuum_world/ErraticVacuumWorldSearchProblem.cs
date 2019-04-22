using ailib.Algorithms.Search.NonDeterministic;
using vacuum_world.Actions;

namespace vacuum_world
{
    public class ErraticVacuumWorldSearchProblem : INonDeterministicSearchProblem<VacuumWorldState, VacuumWorldAction>
    {
        private VacuumWorldState _initialState;

        public ErraticVacuumWorldSearchProblem(VacuumWorldState initialState)
        {
            _initialState = initialState;
        }

        public bool IsGoal(VacuumWorldState state)
        {
            return state.NumberOfDirtySquares() == 0;
        }
    }
}