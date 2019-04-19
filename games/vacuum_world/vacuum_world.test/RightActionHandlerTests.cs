using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test
{
    public class RightActionHandlerTests
    {
        private RightActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new RightActionHandler();
            _state = new VacuumWorldState(3);
        }

        [Test]
        public void GivenNonRightAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Down), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Move_right_from_0_should_go_to_1()
        {
            _handler.DoAction(_state, VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(1, 0), _state.VacuumPos);
        }
        
        [Test]
        public void Move_right_from_max_should_stay_at_max()
        {
            _state.VacuumPos = new Point2D(2, 0);
            
            _handler.DoAction(_state, VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(2, 0), _state.VacuumPos);
        }
    }
}