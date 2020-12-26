using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world
{
    /// <summary>
    /// Vacuum world state machine
    /// </summary>
    public class VacuumWorld
    {
        public VacuumWorldState State { get; }

        private readonly IVacuumWorldActionHandler _actionHandler;

        public VacuumWorld(VacuumWorldState state, IVacuumWorldActionHandler actionHandler)
        {
            State = state.Clone();
            _actionHandler = actionHandler;
        }

        public static VacuumWorld CreateDeterministicVacuumWorld(VacuumWorldState state)
        {
            return new VacuumWorld(state, VacuumWorldActionHandler.CreateDeterministicActionHandler());
        }
        
        public static VacuumWorld CreateErraticVacuumWorld(VacuumWorldState state)
        {
            return new VacuumWorld(state, VacuumWorldActionHandler.CreateErraticWorldActionHandler());
        }

        public void DoAction(VacuumWorldAction action)
        {
            _actionHandler.DoAction(State, action);
        }
    }
}
