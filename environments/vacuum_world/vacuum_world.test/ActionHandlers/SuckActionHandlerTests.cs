using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test.ActionHandlers
{
    public class SuckActionHandlerTests
    {
        private SuckActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new SuckActionHandler();
            _state = new VacuumWorldState(3);
        }

        [Test]
        public void GivenNonSuckAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Down), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Suck_dirty_should_make_it_clean()
        {
            _state.MakeSquareDirty(1, 1);
            _state.VacuumPos = new Point2D(1, 1);
            
            _handler.DoAction(_state, VacuumWorldAction.Suck);
         
            Assert.IsFalse(_state.SquareIsDirty(_state.VacuumPos));
        }
    }
}