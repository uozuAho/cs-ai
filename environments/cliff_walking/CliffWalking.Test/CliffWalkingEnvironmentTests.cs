using System;
using NUnit.Framework;

namespace CliffWalking.Test
{
    internal class CliffWalkingEnvironmentTests
    {
        [Test]
        public void AtStart_ActionSpace_IsUpAndRight()
        {
            var env = new CliffWalkingEnvironment();

            var actions = env.ActionSpace();

            CollectionAssert.AreEquivalent(new[]
                {CliffWalkingAction.Up, CliffWalkingAction.Right}, actions);
        }

        [Test]
        public void FromStart_StepUp()
        {
            var env = new CliffWalkingEnvironment();

            var (observation, reward, isDone) = env.Step(CliffWalkingAction.Up);

            Assert.AreEqual(new Position(0, 1), observation);
            Assert.AreEqual(false, isDone);
            Assert.AreEqual(-1, reward);
        }

        [Test]
        public void AttemptInvalidAction_Throws()
        {
            var env = new CliffWalkingEnvironment();

            Assert.Throws<InvalidOperationException>(() => env.Step(CliffWalkingAction.Left));
        }

        [Test]
        public void StepIntoCliff_RewardsNegative100()
        {
            var env = new CliffWalkingEnvironment();

            var step = env.Step(CliffWalkingAction.Right);

            Assert.AreEqual(-100, step.Reward);
        }

        [Test]
        public void StepIntoCliff_ResetsPositionToStart()
        {
            var bottomLeft = new Position(0, 0);
            var env = new CliffWalkingEnvironment();

            var step = env.Step(CliffWalkingAction.Right);

            Assert.AreEqual(bottomLeft, step.Observation);
        }
    }
}
