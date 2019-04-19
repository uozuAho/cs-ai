using System.Collections.Generic;
using vacuum_world.Actions;

namespace vacuum_world.ActionHandlers
{
    public class VacuumWorldActionHandler : IVacuumWorldActionHandler
    {
        private readonly Dictionary<VacuumWorldAction, IVacuumWorldActionHandler> _actionHandlers;

        public VacuumWorldActionHandler(Dictionary<VacuumWorldAction, IVacuumWorldActionHandler> actionHandlers)
        {
            _actionHandlers = actionHandlers;
        }

        public static VacuumWorldActionHandler CreateDefaultActionHandler()
        {
            return new VacuumWorldActionHandler(
                new Dictionary<VacuumWorldAction, IVacuumWorldActionHandler>
                {
                    {VacuumWorldAction.Left, new LeftActionHandler()},
                    {VacuumWorldAction.Right, new RightActionHandler()},
                    {VacuumWorldAction.Down, new DownActionHandler()},
                    {VacuumWorldAction.Up, new UpActionHandler()},
                    {VacuumWorldAction.Suck, new SuckActionHandler()}
                });
        }
        
        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            var handler = _actionHandlers[action];
            handler.DoAction(state, action);
        }
    }
}