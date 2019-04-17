namespace vacuum_world
{
    public interface IVacuumWorldActionHandler
    {
        void DoAction(VacuumWorldState state, VacuumWorldAction action);
    }
}