using NUnit.Framework;

namespace vacuum_world.test
{
    public class ErraticVacuumWorldSearchProblemTests
    {
        private ErraticVacuumWorldSearchProblem _searchProblem;

        [SetUp]
        public void Setup()
        {
            var initialState = new VacuumWorldState(3);
            _searchProblem = new ErraticVacuumWorldSearchProblem(initialState);
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
    }
}