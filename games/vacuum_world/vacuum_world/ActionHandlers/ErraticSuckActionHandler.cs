using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    /// <summary>
    /// Implements the 'erratic' vacuum world behaviours, where actions
    /// are non deterministic. In this world, the 'suck' action may:
    /// - when applied to a dirty square, cleans that square, and may clean an adjacent square
    /// - when applied to a clean square, may make that square dirty
    /// </summary>
    public class ErraticSuckActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            throw new System.NotImplementedException();
        }
    }
}