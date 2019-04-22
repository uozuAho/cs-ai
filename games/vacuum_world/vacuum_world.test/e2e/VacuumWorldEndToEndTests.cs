using ailib.Algorithms.Search.NonDeterministic;
using NUnit.Framework;
using vacuum_world.Actions;

namespace vacuum_world.test.e2e
{
    public class VacuumWorldEndToEndTests
    {
        [Test]
        public void SolveErratic2X2VacuumWorldWithDfs()
        {
            var initialState = new VacuumWorldState(2);
            initialState.MakeSquareDirty(1, 1);

            var problem = new ErraticVacuumWorldSearchProblem(initialState);
            var solver = new NonDeterministicDfsSearch<VacuumWorldState, VacuumWorldAction>(problem);
            var solution = solver.GetSolution();
            
            var actionCounter = 0;
            const int actionLimit = 1000;
            var erraticWorld = VacuumWorld.CreateErraticVacuumWorld(initialState);
            
            while (!problem.IsGoal(erraticWorld.State))
            {
                var action = solution.NextAction(erraticWorld.State);

                erraticWorld.DoAction(action);
                
                if (++actionCounter > actionLimit)
                    Assert.Fail($"failed to reach goal after {actionCounter} actions");
            }
        }
    }
}