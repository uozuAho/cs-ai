using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test
{
    public class LeftActionHandlerTests
    {
        private LeftActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new LeftActionHandler();
            _state = new VacuumWorldState(3);
        }

        [Test]
        public void GivenNonLeftAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Down), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Move_left_from_0_should_stay_at_0()
        {
            _handler.DoAction(_state, VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), _state.VacuumPos);
        }
        
        [Test]
        public void Move_left_from_1_should_go_to_0()
        {
            _state.VacuumPos = new Point2D(1, 0);
            
            _handler.DoAction(_state, VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), _state.VacuumPos);
        }
    }
}