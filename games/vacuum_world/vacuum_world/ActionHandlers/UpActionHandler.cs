using System;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class UpActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Up) throw new InvalidOperationException();
            
            if (state.VacuumPos.Y > 0)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y - 1);
            }
        }
    }
}