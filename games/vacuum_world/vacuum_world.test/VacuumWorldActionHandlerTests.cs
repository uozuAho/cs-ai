using NUnit.Framework;

namespace vacuum_world.test
{
    public class VacuumWorldActionHandlerTests
    {
        private VacuumWorldActionHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new VacuumWorldActionHandler();
        }
        
        [Test]
        public void GivenVacuumIsOnDirtySquare_Suck_ShouldMakeSquareClean()
        {
            var state = new VacuumWorldState(3);
            state.SetSquareIsDirty(1, 1, true);
            state.VacuumPos = new Point2D(1, 1);
            
            _handler.DoAction(state, VacuumWorldAction.Suck);
         
            Assert.IsFalse(state.SquareIsDirty(1, 1));
        }
        
        [Test]
        public void Suck_dirty_should_make_it_clean()
        {
            var state = new VacuumWorldState(3);
            state.SetSquareIsDirty(1, 1, true);
            state.VacuumPos = new Point2D(1, 1);
            
            _handler.DoAction(state, VacuumWorldAction.Suck);
         
            Assert.IsFalse(state.SquareIsDirty(1, 1));
        }
        
        [Test]
        public void Move_left_from_0_should_stay_at_0()
        {
            var state = new VacuumWorldState(3);
            
            _handler.DoAction(state, VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), state.VacuumPos);
        }
        
        [Test]
        public void Move_left_from_1_should_go_to_0()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(1, 0)};
            
            _handler.DoAction(state, VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), state.VacuumPos);
        }
        
        [Test]
        public void Move_right_from_0_should_go_to_1()
        {
            var state = new VacuumWorldState(3);
            
            _handler.DoAction(state, VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(1, 0), state.VacuumPos);
        }
        
        [Test]
        public void Move_right_from_max_should_stay_at_max()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(2, 0)};
            
            _handler.DoAction(state, VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(2, 0), state.VacuumPos);
        }
        
        [Test]
        public void Move_down_from_0_should_go_to_1()
        {
            var state = new VacuumWorldState(3);
            
            _handler.DoAction(state, VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 1), state.VacuumPos);
        }
        
        [Test]
        public void Move_down_from_max_should_stay_at_max()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(0, 2)};
            
            _handler.DoAction(state, VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 2), state.VacuumPos);
        }
        
        [Test]
        public void Move_up_from_0_should_stay_at_0()
        {
            var state = new VacuumWorldState(3);
            
            _handler.DoAction(state, VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 0), state.VacuumPos);
        }
        
        [Test]
        public void Move_up_from_2_should_go_to_1()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(0, 2)};
            
            _handler.DoAction(state, VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 1), state.VacuumPos);
        }
    }
}