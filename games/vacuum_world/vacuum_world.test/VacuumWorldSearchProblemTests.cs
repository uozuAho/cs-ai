using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test
{
    public class VacuumWorldSearchProblemTests
    {
        private VacuumWorldSearchProblem _problem;
        private VacuumWorldState _initialState;

        [SetUp]
        public void Setup()
        {
            _initialState = new VacuumWorldState(2);
            _initialState.MakeSquareDirty(0, 0);
            _initialState.MakeSquareDirty(0, 1);

            _problem = new VacuumWorldSearchProblem(_initialState,
                VacuumWorldActionHandler.CreateDeterministicActionHandler());
        }
        
        [Test]
        public void InitialState()
        {
            Assert.AreEqual(_initialState, _problem.InitialState);
        }
        
        [Test]
        public void IsGoal_should_be_false_for_state_with_dirty_squares()
        {
            Assert.IsFalse(_problem.IsGoal(_initialState));
        }
        
        [Test]
        public void IsGoal_should_be_true_for_state_with_no_dirty_squares()
        {
            Assert.IsTrue(_problem.IsGoal(new VacuumWorldState(2)));
        }
        
        [Test]
        public void All_actions_should_be_available()
        {
            var actions = _problem.GetActions(_initialState);
            var expectedActions = new[]
            {
                VacuumWorldAction.Suck, 
                VacuumWorldAction.Left, 
                VacuumWorldAction.Right,
                VacuumWorldAction.Down,
                VacuumWorldAction.Up
            };

            CollectionAssert.AreEquivalent(expectedActions, actions);
        }
        
        [Test]
        public void DoAction_right_should_move_right()
        {
            var nextState = _problem.DoAction(_initialState, VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(1, 0), nextState.VacuumPos);
        }

        [Test]
        public void DoAction_ShouldNotModifyGivenState()
        {
            var state = new VacuumWorldState(3);

            var newState = _problem.DoAction(state, VacuumWorldAction.Down);
            
            Assert.IsFalse(ReferenceEquals(newState, state));
            Assert.IsFalse(newState.Equals(state));
        }
    }
}
