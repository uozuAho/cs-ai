using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public interface IVacuumWorldActionHandler
    {
        void DoAction(VacuumWorldState state, VacuumWorldAction action);
    }
}