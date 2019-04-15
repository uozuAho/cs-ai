using System;
using System.Collections.Generic;

namespace vacuum_world
{
    /// <summary>
    /// The fully observable, deterministic vacuum world from AI: a modern approach
    /// </summary>
    public class VacuumWorld : IVacuumWorld
    {
        public VacuumWorldState State => _state.Clone();
        
        private readonly VacuumWorldState _state;
        private readonly IVacuumWorldActionHandler _actionHandler;

        private static readonly Dictionary<VacuumWorldAction, Func<VacuumWorldState, VacuumWorldState>> ActionHandlers
            =
            new Dictionary<VacuumWorldAction, Func<VacuumWorldState, VacuumWorldState>>
            {
                {VacuumWorldAction.Left, DoLeft},
                {VacuumWorldAction.Right, DoRight},
                {VacuumWorldAction.Down, DoDown},
                {VacuumWorldAction.Up, DoUp},
                {VacuumWorldAction.Suck, DoSuck}
            };

        public VacuumWorld(VacuumWorldState state, IVacuumWorldActionHandler actionHandler)
        {
            _state = state.Clone();
            _actionHandler = actionHandler;
        }

        public void DoAction(VacuumWorldAction action)
        {
            _actionHandler.DoAction(_state, action);
        }

        public static VacuumWorldState PeekAction(VacuumWorldState state, VacuumWorldAction action)
        {
            var handler = ActionHandlers[action];
            return handler(state);
        }

        private static VacuumWorldState DoLeft(VacuumWorldState state) 
        {
            var newState = state.Clone();
            if (newState.VacuumPos.X > 0)
            {
                newState.VacuumPos = new Point2D(state.VacuumPos.X - 1, state.VacuumPos.Y);
            }
            return newState;
        }
        
        private static VacuumWorldState DoRight(VacuumWorldState state) 
        {
            var newState = state.Clone();
            if (newState.VacuumPos.X < state.WorldSize - 1)
            {
                newState.VacuumPos = new Point2D(state.VacuumPos.X + 1, state.VacuumPos.Y);
            }
            return newState;
        }
        
        private static VacuumWorldState DoDown(VacuumWorldState state) 
        {
            var newState = state.Clone();
            if (newState.VacuumPos.Y < state.WorldSize - 1)
            {
                newState.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y + 1);
            }
            return newState;
        }
        
        private static VacuumWorldState DoUp(VacuumWorldState state) 
        {
            var newState = state.Clone();
            if (newState.VacuumPos.Y > 0)
            {
                newState.VacuumPos = new Point2D(state.VacuumPos.X, state.VacuumPos.Y - 1);
            }
            return newState;
        }

        private static VacuumWorldState DoSuck(VacuumWorldState state) 
        {
            var newState = state.Clone();
            newState.SetSquareIsDirty(newState.VacuumPos.X, newState.VacuumPos.Y, false);
            return newState;
        }
    }

    public interface IVacuumWorld
    {
        /// <summary>
        /// Perform the action and update the world state
        /// </summary>
        void DoAction(VacuumWorldAction action);
    }
}
