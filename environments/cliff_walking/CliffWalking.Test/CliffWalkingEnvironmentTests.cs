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
            var upAndRight = new[]
                {CliffWalkingAction.Up, CliffWalkingAction.Right};

            var actions = env.ActionSpace();

            CollectionAssert.AreEquivalent(upAndRight, actions);
        }

        [Test]
        public void InMiddle_ActionSpace_IsAllDirections()
        {
            var middle = new Position(6, 2);
            var env = new CliffWalkingEnvironment(middle);
            var allDirections = Enum.GetValues(typeof(CliffWalkingAction));

            var actions = env.ActionSpace();

            CollectionAssert.AreEquivalent(allDirections, actions);
        }

        [Test]
        public void AtTopRight_ActionSpace_IsDownAndLeft()
        {
            var topRight = new Position(11, 3);
            var env = new CliffWalkingEnvironment(topRight);
            var downAndLeft = new[]
                {CliffWalkingAction.Down, CliffWalkingAction.Left};

            var actions = env.ActionSpace();

            CollectionAssert.AreEquivalent(downAndLeft, actions);
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

        [TestCase(0, 0, CliffWalkingAction.Right)]
        [TestCase(1, 1, CliffWalkingAction.Down)]
        [TestCase(5, 1, CliffWalkingAction.Down)]
        public void StepIntoCliff_ResetsPositionToStart_AndRewardsNegative100(
            int startX, int startY, CliffWalkingAction action)
        {
            var bottomLeft = new Position(0, 0);
            var startingPosition = new Position(startX, startY);
            var env = new CliffWalkingEnvironment(startingPosition);

            var (observation, reward, _) = env.Step(action);

            Assert.AreEqual(bottomLeft, observation);
            Assert.AreEqual(-100, reward);
        }
    }
}
