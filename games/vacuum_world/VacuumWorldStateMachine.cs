using System;
using System.Collections.Generic;

namespace vacuum_world
{
    public class VacuumWorldStateMachine
    {
        public VacuumWorldState State => _state.Clone();
        
        private VacuumWorldState _state;

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

        public VacuumWorldStateMachine(VacuumWorldState state)
        {
            _state = state.Clone();
        }

        /// <summary>
        /// Perform the action and update the state machine
        /// </summary>
        public void DoAction(VacuumWorldAction action)
        {
            var newState = PeekAction(_state, action);
            _state = newState;
        }

        /// <summary>
        /// Get the resultant state from performing the given action on the given state
        /// </summary>
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
            newState.GetSquare(newState.VacuumPos).IsDirty = false;
            return newState;
        }
    }
}
