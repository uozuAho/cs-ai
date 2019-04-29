using System;
using System.Collections.Generic;
using System.Linq;
using ailib.Algorithms.Search.NonDeterministic;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world
{
    public class ErraticVacuumWorldSearchProblem : INonDeterministicSearchProblem<VacuumWorldState, VacuumWorldAction>
    {
        private readonly VacuumWorldActionHandler _actionHandler;

        public ErraticVacuumWorldSearchProblem()
        {
            _actionHandler = VacuumWorldActionHandler.CreateErraticWorldActionHandler();
        }

        public bool IsGoal(VacuumWorldState state)
        {
            return state.NumberOfDirtySquares() == 0;
        }

        public IEnumerable<VacuumWorldAction> GetActions(VacuumWorldState state)
        {
            return Enum.GetValues(typeof(VacuumWorldAction)).Cast<VacuumWorldAction>();
        }

        public IEnumerable<VacuumWorldState> DoAction(VacuumWorldState state, VacuumWorldAction action)
        {
            if (action != VacuumWorldAction.Suck)
            {
                var newState = state.Clone();
                _actionHandler.DoAction(newState, action);
                yield return newState;
            }
            else
            {
                // todo: clean me
                if (state.SquareIsDirty(state.VacuumPos))
                {
                    var newStateCleaned = state.Clone();
                    newStateCleaned.CleanSquare(state.VacuumPos);
                    yield return newStateCleaned;

                    var dirtyNeighbours = state
                        .AdjacentSquares(state.VacuumPos)
                        .Where(state.SquareIsDirty);
                    
                    foreach (var neighbour in dirtyNeighbours)
                    {
                        var neighbourCleanedState = newStateCleaned.Clone();
                        neighbourCleanedState.CleanSquare(neighbour);
                        yield return neighbourCleanedState;
                    }
                }
                else
                {
                    yield return state.Clone();
                    
                    var dirtyState = state.Clone();
                    dirtyState.MakeSquareDirty(dirtyState.VacuumPos);
                    yield return dirtyState;
                }
            }
        }
    }
}