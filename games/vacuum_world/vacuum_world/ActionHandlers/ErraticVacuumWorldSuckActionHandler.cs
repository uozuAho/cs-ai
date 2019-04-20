using System;
using System.Linq;
using vacuum_world.Actions;
using vacuum_world.Utils;

namespace vacuum_world.ActionHandlers
{
    public class ErraticVacuumWorldSuckActionHandler : IVacuumWorldActionHandler
    {
        private readonly IVacuumWorldActionHandler _decoratedHandler;
        private readonly double _cleanExtraProbability;
        private readonly Random _random = new Random();

        public ErraticVacuumWorldSuckActionHandler(IVacuumWorldActionHandler decoratedHandler,
            double cleanExtraProbability = 0.5)
        {
            _decoratedHandler = decoratedHandler;
            _cleanExtraProbability = cleanExtraProbability;
        }

        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Suck) throw new InvalidOperationException();
            
            _decoratedHandler.DoAction(state, action);

            if (_random.TrueWithProbability(_cleanExtraProbability))
            {
                CleanRandomDirtyNeighbour(state);
            }
        }

        private void CleanRandomDirtyNeighbour(VacuumWorldState state)
        {
            var adj = state.AdjacentSquares(state.VacuumPos).Where(state.SquareIsDirty).ToList();

            if (adj.Count == 0) return;
            
            var squareToClean = _random.Choice(adj);
            state.CleanSquare(squareToClean);
        }
    }
}