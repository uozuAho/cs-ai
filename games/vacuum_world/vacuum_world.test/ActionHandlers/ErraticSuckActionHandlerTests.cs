using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test.ActionHandlers
{
    public class ErraticSuckActionHandlerTests
    {
        private ErraticVacuumWorldSuckActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _handler = new ErraticVacuumWorldSuckActionHandler();
            _state = new VacuumWorldState(3);
        }
        
        [Test]
        public void GivenNonSuckAction_DoAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Up), Throws.InvalidOperationException);
        }

        [Test]
        public void GivenSquareIsDirty_Suck_ShouldCleanSquare()
        {
            _state.VacuumPos = new Point2D(1, 1);
            _state.SetSquareIsDirty(_state.VacuumPos, true);
            
            _handler.DoAction(_state, VacuumWorldAction.Suck);
            
            Assert.IsFalse(_state.SquareIsDirty(_state.VacuumPos));
        }
    }
}