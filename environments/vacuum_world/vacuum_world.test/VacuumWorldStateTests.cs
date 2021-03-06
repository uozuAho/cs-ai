using NUnit.Framework;

namespace vacuum_world.test
{
    [TestFixture]
    public class VacuumWorldStateTests
    {
        [Test]
        public void New_empty_states_should_be_equal()
        {
            var a = new VacuumWorldState(3);
            var b = new VacuumWorldState(3);
            
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
        
        [Test]
        public void States_with_same_data_should_be_equal()
        {
            var a = new VacuumWorldState(3);
            var b = new VacuumWorldState(3);
            
            a.VacuumPos = new Point2D(1, 2);
            a.MakeSquareDirty(1, 1);
            b.VacuumPos = new Point2D(1, 2);
            b.MakeSquareDirty(1, 1);
            
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
        
        [Test]
        public void States_with_different_vacuum_pos_should_not_be_equal()
        {
            var a = new VacuumWorldState(3);
            var b = new VacuumWorldState(3);
            
            a.VacuumPos = new Point2D(1, 2);
            
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
        
        [Test]
        public void States_with_different_dirty_squares_should_not_be_equal()
        {
            var a = new VacuumWorldState(3);
            var b = new VacuumWorldState(3);

            a.MakeSquareDirty(1, 1);
            
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
