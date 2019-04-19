using System;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class LeftActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Left) throw new InvalidOperationException();
            
            if (state.VacuumPos.X > 0)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X - 1, state.VacuumPos.Y);
            }
        }
    }
}