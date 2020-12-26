using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.test.ActionHandlers
{
    public class ErraticSuckActionHandlerTests
    {
        private IVacuumWorldActionHandler _decoratedHandler;
        private ErraticSuckActionHandler _handler;
        private VacuumWorldState _state;

        [SetUp]
        public void Setup()
        {
            _decoratedHandler = A.Fake<IVacuumWorldActionHandler>();
            _handler = new ErraticSuckActionHandler(_decoratedHandler);
            _state = new VacuumWorldState(3);
        }
        
        [Test]
        public void GivenNonSuckAction_DoAction_ShouldThrowInvalidOperation()
        {
            Assert.That(() => _handler.DoAction(_state, VacuumWorldAction.Up), Throws.InvalidOperationException);
        }

        [Test]
        public void DoAction_ShouldCallDecoratedHandler()
        {
            _handler.DoAction(_state, VacuumWorldAction.Suck);

            A.CallTo(() => _decoratedHandler.DoAction(_state, VacuumWorldAction.Suck)).MustHaveHappened();
        }
        
        [Test]
        public void GivenCleanExtraProbabilityOf1_Suck_ShouldCleanAdjacentSquare()
        {
            // arrange
            _state.SetAllSquaresDirty();
            _state.VacuumPos = new Point2D(1, 1);
            
            const double cleanExtraProbability = 1.0;
            _handler = new ErraticSuckActionHandler(_decoratedHandler, cleanExtraProbability);
            
            // act
            _handler.DoAction(_state, VacuumWorldAction.Suck);
            
            // assert
            var numDirtyNeighbours = _state.AdjacentSquares(_state.VacuumPos).Count(s => _state.SquareIsDirty(s));
            Assert.AreEqual(3, numDirtyNeighbours);
        }
        
        [Test]
        public void GivenCleanExtraProbabilityOf0_Suck_ShouldNotCleanAdjacentSquare()
        {
            // arrange
            _state.SetAllSquaresDirty();
            _state.VacuumPos = new Point2D(1, 1);
            
            const double cleanExtraProbability = 0.0;
            _handler = new ErraticSuckActionHandler(_decoratedHandler, cleanExtraProbability);
            
            // act
            _handler.DoAction(_state, VacuumWorldAction.Suck);
            
            // assert
            var numDirtyNeighbours = _state.AdjacentSquares(_state.VacuumPos).Count(s => _state.SquareIsDirty(s));
            Assert.AreEqual(4, numDirtyNeighbours);
        }

        [Test]
        public void GivenVacuumIsOnCleanSquare_AndMakeDirtyProbabilityOf1_Suck_ShouldMakeSquareDirty()
        {
            // arrange
            _state.SetAllSquaresClean();
            _state.VacuumPos = new Point2D(1, 1);
            
            const double cleanExtraProbability = 0.0;
            const double makeDirtyProbability = 1.0;
            _handler = new ErraticSuckActionHandler(_decoratedHandler, cleanExtraProbability, makeDirtyProbability);
            
            // act
            _handler.DoAction(_state, VacuumWorldAction.Suck);
            
            // assert
            Assert.IsTrue(_state.SquareIsDirty(_state.VacuumPos));
        }
    }
}