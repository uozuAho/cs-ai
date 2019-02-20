using NUnit.Framework;

namespace vacuum_world.test
{
    public class VacuumWorldStateMachineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Suck_dirty_should_make_it_clean()
        {
            var state = new VacuumWorldState(3);
            state.GetSquare(1, 1).IsDirty = true;
            state.VacuumPos = new Point2D(1, 1);
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Suck);
         
            Assert.IsFalse(machine.State.GetSquare(1, 1).IsDirty);
        }
        
        [Test]
        public void Move_left_from_0_should_stay_at_0()
        {
            var machine = new VacuumWorldStateMachine(new VacuumWorldState(3));
            
            machine.DoAction(VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_left_from_1_should_go_to_0()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(1, 0)};
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Left);
            
            Assert.AreEqual(new Point2D(0, 0), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_right_from_0_should_go_to_1()
        {
            var state = new VacuumWorldState(3);
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(1, 0), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_right_from_max_should_stay_at_max()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(2, 0)};
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Right);
            
            Assert.AreEqual(new Point2D(2, 0), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_down_from_0_should_go_to_1()
        {
            var state = new VacuumWorldState(3);
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 1), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_down_from_max_should_stay_at_max()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(0, 2)};
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Down);
            
            Assert.AreEqual(new Point2D(0, 2), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_up_from_0_should_stay_at_0()
        {
            var state = new VacuumWorldState(3);
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 0), machine.State.VacuumPos);
        }
        
        [Test]
        public void Move_up_from_2_should_go_to_1()
        {
            var state = new VacuumWorldState(3) {VacuumPos = new Point2D(0, 2)};
            var machine = new VacuumWorldStateMachine(state);
            
            machine.DoAction(VacuumWorldAction.Up);
            
            Assert.AreEqual(new Point2D(0, 1), machine.State.VacuumPos);
        }
    }
}
