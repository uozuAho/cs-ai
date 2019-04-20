using System;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class DownActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Down) throw new InvalidOperationException();
            
            if (state.VacuumPos.Y < state.WorldSize - 1)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y + 1);
            }
        }
    }
}