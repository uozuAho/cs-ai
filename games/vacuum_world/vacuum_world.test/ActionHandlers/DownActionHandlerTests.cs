using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test.ActionHandlers
{
    public class DownActionHandlerTests
    {
        private DownActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new DownActionHandler();
            _state = new VacuumWorldState(3);
        }

        [Test]
        public void GivenNonDownAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Up), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Move_down_from_0_should_go_to_1()
        {
            _handler.DoAction(_state, VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 1), _state.VacuumPos);
        }
        
        [Test]
        public void Move_down_from_max_should_stay_at_max()
        {
            _state.VacuumPos = new Point2D(0, 2);
            
            _handler.DoAction(_state, VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 2), _state.VacuumPos);
        }
    }
}