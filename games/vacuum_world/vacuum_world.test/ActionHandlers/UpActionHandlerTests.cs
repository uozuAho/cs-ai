using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test.ActionHandlers
{
    public class UpActionHandlerTests
    {
        private UpActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new UpActionHandler();
            _state = new VacuumWorldState(3);
        }

        [Test]
        public void GivenNonUpAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Down), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Move_up_from_0_should_stay_at_0()
        {
            _handler.DoAction(_state, VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 0), _state.VacuumPos);
        }
        
        [Test]
        public void Move_up_from_2_should_go_to_1()
        {
            _state.VacuumPos = new Point2D(0, 2);
            
            _handler.DoAction(_state, VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 1), _state.VacuumPos);
        }
    }
}