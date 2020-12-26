using System;
using System.Linq;
using vacuum_world.Actions;
using vacuum_world.Utils;

namespace vacuum_world.ActionHandlers
{
    public class ErraticSuckActionHandler : IVacuumWorldActionHandler
    {
        private readonly IVacuumWorldActionHandler _decoratedHandler;
        private readonly double _cleanExtraProbability;
        private readonly double _makeDirtyProbability;
        private readonly Random _random = new Random();

        public ErraticSuckActionHandler(IVacuumWorldActionHandler decoratedHandler,
            double cleanExtraProbability = 0.5, double makeDirtyProbability = 0.5)
        {
            _decoratedHandler = decoratedHandler;
            _cleanExtraProbability = cleanExtraProbability;
            _makeDirtyProbability = makeDirtyProbability;
        }

        public void DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Suck) throw new InvalidOperationException();

            var squareWasClean = !state.SquareIsDirty(state.VacuumPos);
            
            _decoratedHandler.DoAction(state, action);

            if (squareWasClean)
            {
                if (_random.TrueWithProbability(_makeDirtyProbability))
                {
                    state.MakeSquareDirty(state.VacuumPos);
                }
            }
            else
            {
                if (_random.TrueWithProbability(_cleanExtraProbability))
                {
                    CleanRandomDirtyNeighbour(state);
                }
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