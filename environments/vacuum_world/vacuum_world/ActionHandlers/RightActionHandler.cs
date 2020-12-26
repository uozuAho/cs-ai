using System;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class RightActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Right) throw new InvalidOperationException();
            
            if (state.VacuumPos.X < state.WorldSize - 1)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X + 1, state.VacuumPos.Y);
            }
        }
    }
}