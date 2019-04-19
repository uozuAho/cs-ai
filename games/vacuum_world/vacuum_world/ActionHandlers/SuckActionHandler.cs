using System;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class SuckActionHandler : IVacuumWorldActionHandler
    {
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Suck) throw new InvalidOperationException();
            
            state.SetSquareIsDirty(state.VacuumPos.X, state.VacuumPos.Y, false);
        }
    }
}