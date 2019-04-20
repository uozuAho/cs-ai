using System;
using System.Collections.Generic;
using System.Linq;
using vacuum_world.Actions;

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

            if (TrueWithProbability(_cleanExtraProbability))
            {
                CleanRandomDirtyNeighbour(state);
            }
        }

        private void CleanRandomDirtyNeighbour(VacuumWorldState state)
        {
            var adj = state.AdjacentSquares(state.VacuumPos).Where(state.SquareIsDirty).ToList();

            if (adj.Count == 0) return;
            
            var squareToClean = RandomChoice(adj);
            state.SetSquareIsDirty(squareToClean, false);
        }

        private T RandomChoice<T>(IReadOnlyList<T> items)
        {
            var idx = _random.Next(0, items.Count - 1);
            return items[idx];
        }

        private bool TrueWithProbability(double probability)
        {
            return _random.NextDouble() < probability;
        }
    }
}