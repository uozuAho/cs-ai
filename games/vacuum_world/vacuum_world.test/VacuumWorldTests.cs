using FakeItEasy;
using NUnit.Framework;

namespace vacuum_world.test
{
    public class VacuumWorldTests
    {
        [Test]
        public void DoAction_ShouldCallActionHandler()
        {
            var actionHandler = A.Fake<IVacuumWorldActionHandler>();
            var vacuumWorld = new VacuumWorld(new VacuumWorldState(3), actionHandler);
            const VacuumWorldAction action = VacuumWorldAction.Up;

            vacuumWorld.DoAction(action);

            A.CallTo(() => actionHandler.DoAction(A<VacuumWorldState>._, action)).MustHaveHappened();
        }
    }
}
