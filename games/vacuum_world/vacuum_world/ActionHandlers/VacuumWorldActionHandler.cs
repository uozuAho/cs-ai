using System;
using System.Collections.Generic;

namespace vacuum_world
{
    public class VacuumWorldActionHandler : IVacuumWorldActionHandler
    {
        private static readonly Dictionary<VacuumWorldAction, Action<VacuumWorldState>> ActionHandlers =
            new Dictionary<VacuumWorldAction, Action<VacuumWorldState>>
            {
                {VacuumWorldAction.Left, DoLeft},
                {VacuumWorldAction.Right, DoRight},
                {VacuumWorldAction.Down, DoDown},
                {VacuumWorldAction.Up, DoUp},
                {VacuumWorldAction.Suck, DoSuck}
            };
        
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            var handler = ActionHandlers[action];
            handler(state);
        }

        private static void DoLeft(VacuumWorldState state)
        {
            if (state.VacuumPos.X > 0)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X - 1, state.VacuumPos.Y);
            }
        }
        
        private static void DoRight(VacuumWorldState state) 
        {
            if (state.VacuumPos.X < state.WorldSize - 1)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X + 1, state.VacuumPos.Y);
            }
        }
        
        private static void DoDown(VacuumWorldState state) 
        {
            if (state.VacuumPos.Y < state.WorldSize - 1)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y + 1);
            }
        }
        
        private static void DoUp(VacuumWorldState state) 
        {
            if (state.VacuumPos.Y > 0)
            {
                state.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y - 1);
            }
        }

        private static void DoSuck(VacuumWorldState state) 
        {
            state.SetSquareIsDirty(state.VacuumPos.X, state.VacuumPos.Y, false);
        }
    }
}