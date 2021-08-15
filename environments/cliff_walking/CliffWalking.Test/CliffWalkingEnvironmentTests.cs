using System;
using NUnit.Framework;

namespace CliffWalking.Test
{
    internal class CliffWalkingEnvironmentTests
    {
        private static readonly Position StartingPosition = new(0, 0);
        private static readonly Position GoalPosition = new (11, 0);
        private static readonly Position SomewhereInTheMiddle = new (6, 2);
        private static readonly Position TopRight = new (11, 3);

        [Test]
        public void AtStart_ActionSpace_IsUpAndRight()
        {
            var upAndRight = new[] {CliffWalkingAction.Up, CliffWalkingAction.Right};

            var actions = CliffWalkingEnvironment.ActionSpace(StartingPosition);

            CollectionAssert.AreEquivalent(upAndRight, actions);
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
        public void InMiddle_ActionSpace_IsAllDirections()
        {
            var allDirections = Enum.GetValues(typeof(CliffWalkingAction));
        
            var actions = CliffWalkingEnvironment.ActionSpace(SomewhereInTheMiddle);
        
            CollectionAssert.AreEquivalent(allDirections, actions);
        }

        [Test]
        public void InMiddle_StepRight_MovesRight()
        {
            var env = new CliffWalkingEnvironment(SomewhereInTheMiddle);
            var expectedPosition = new Position(SomewhereInTheMiddle.X + 1, SomewhereInTheMiddle.Y);

            var (observation, reward, isDone) = env.Step(CliffWalkingAction.Right);

            Assert.AreEqual(expectedPosition, observation);
            Assert.AreEqual(reward, -1);
            Assert.AreEqual(false, isDone);
        }

        [Test]
        public void AtTopRight_ActionSpace_IsDownAndLeft()
        {
            var downAndLeft = new[] {CliffWalkingAction.Down, CliffWalkingAction.Left};
        
            var actions = CliffWalkingEnvironment.ActionSpace(TopRight);
        
            CollectionAssert.AreEquivalent(downAndLeft, actions);
        }

        [Test]
        public void StepIntoGoalPosition_IsDone()
        {
            var justAboveGoal = new Position(GoalPosition.X, GoalPosition.Y + 1);
            var env = new CliffWalkingEnvironment(justAboveGoal);

            var (observation, reward, isDone) = env.Step(CliffWalkingAction.Down);

            Assert.AreEqual(true, isDone);
            Assert.AreEqual(GoalPosition, observation);
            Assert.AreEqual(-1, reward);
        }

        [Test]
        public void CannotStart_AtGoal()
        {
            Assert.Throws<ArgumentException>(() =>
                new CliffWalkingEnvironment(GoalPosition));
        }

        [TestCase(0, 0, CliffWalkingAction.Left)]
        [TestCase(0, 3, CliffWalkingAction.Up)]
        [TestCase(11, 1, CliffWalkingAction.Right)]
        public void AttemptInvalidAction_Throws(
            int startX, int startY, CliffWalkingAction action)
        {
            var env = new CliffWalkingEnvironment(new Position(startX, startY));

            Assert.Throws<InvalidOperationException>(() => env.Step(action));
        }

        [TestCase(0, 0, CliffWalkingAction.Right)]
        [TestCase(1, 1, CliffWalkingAction.Down)]
        [TestCase(5, 1, CliffWalkingAction.Down)]
        public void StepIntoCliff_ResetsPositionToStart_AndRewardsNegative100(
            int startX, int startY, CliffWalkingAction action)
        {
            var initialPosition = new Position(startX, startY);
            var env = new CliffWalkingEnvironment(initialPosition);

            var (observation, reward, _) = env.Step(action);

            Assert.AreEqual(StartingPosition, observation);
            Assert.AreEqual(-100, reward);
        }
    }
}
