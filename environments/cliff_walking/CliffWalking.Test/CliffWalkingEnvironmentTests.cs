using NUnit.Framework;

namespace CliffWalking.Test
{
    internal class CliffWalkingEnvironmentTests
    {
        [Test]
        public void Starting_actions_are_up_and_right()
        {
            var env = new CliffWalkingEnvironment();

            var actions = env.ActionSpace();

            CollectionAssert.AreEquivalent(new[]
                {CliffWalkingAction.Up, CliffWalkingAction.Right}, actions);
        }
    }
}
