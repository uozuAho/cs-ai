using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using vacuum_world.Actions;

namespace vacuum_world.test
{
    public class ErraticVacuumWorldSearchProblemTests
    {
        private ErraticVacuumWorldSearchProblem _searchProblem;
        private VacuumWorldState _initialState;

        [SetUp]
        public void Setup()
        {
            _initialState = new VacuumWorldState(3);
            _searchProblem = new ErraticVacuumWorldSearchProblem();
        }
        
        [Test]
        public void NoDirtySquares_ShouldBeGoal()
        {
            var stateWithNoDirtySquares = new VacuumWorldState(3);
            
            Assert.IsTrue(_searchProblem.IsGoal(stateWithNoDirtySquares));
        }

        [Test]
        public void GivenSomeSquaresAreDirty_ShouldNotBeGoalState()
        {
            var stateWithDirtySquares = new VacuumWorldState(3);
            stateWithDirtySquares.MakeSquareDirty(1, 1);
            
            Assert.IsFalse(_searchProblem.IsGoal(stateWithDirtySquares));
        }

        [TestCase(VacuumWorldAction.Up)]
        [TestCase(VacuumWorldAction.Down)]
        [TestCase(VacuumWorldAction.Left)]
        [TestCase(VacuumWorldAction.Right)]
        public void NonSuckAction_ShouldReturnOneState(VacuumWorldAction action)
        {
            var potentialStates = _searchProblem.DoAction(_initialState, action).ToList();
            
            Assert.AreEqual(1, potentialStates.Count);
        }
        
        [Test]
        public void GivenSquareIsClean_Suck_ShouldReturnTwoPossibleStates()
        {
            var currentState = new VacuumWorldState(3);

            var possibleStateAllClean = currentState.Clone();
            var possibleStateOneSquareDirty = currentState.Clone();
            possibleStateOneSquareDirty.MakeSquareDirty(currentState.VacuumPos);
            
            // act
            var potentialStates = _searchProblem.DoAction(currentState, VacuumWorldAction.Suck).ToList();
            
            // assert
            Assert.AreEqual(2, potentialStates.Count);
            Assert.AreEqual(1, potentialStates.Count(s => s.Equals(possibleStateAllClean)));
            Assert.AreEqual(1, potentialStates.Count(s => s.Equals(possibleStateOneSquareDirty)));
        }
        
        [Test]
        public void GivenAllSquaresAreDirty_Suck_ShouldReturnCleanedStatePlusCleanedNeighbourStates()
        {
            var currentState = new VacuumWorldState(3);
            currentState.SetAllSquaresDirty();
            currentState.VacuumPos = new Point2D(1, 1);

            var onlyOneSquareCleanedState = currentState.Clone();
            onlyOneSquareCleanedState.CleanSquare(currentState.VacuumPos);
            var neighbourCleanedStates = CreateAllStatesWithOneCleanedNeighbour(onlyOneSquareCleanedState);

            var expectedPotentialStates = neighbourCleanedStates
                .Concat(new[] {onlyOneSquareCleanedState}).ToList();
            
            // act
            var potentialStates = _searchProblem.DoAction(currentState, VacuumWorldAction.Suck).ToList();
            
            // assert
            CollectionAssert.AreEquivalent(expectedPotentialStates, potentialStates);
        }

        private static IEnumerable<VacuumWorldState> CreateAllStatesWithOneCleanedNeighbour(VacuumWorldState state)
        {
            return state
                .AdjacentSquares(state.VacuumPos)
                .Select(square =>
                {
                    var neighbourState = state.Clone();
                    neighbourState.CleanSquare(square);
                    return neighbourState;
                });
        }
    }
}