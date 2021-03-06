using System;
using NUnit.Framework;

namespace random_walk.Test
{
    public class RandomWalkEnvironmentTests
    {
        [Test]
        public void WhenStartingAtLeftmostPosition_StepLeft_Loses()
        {
            var env = new RandomWalkEnvironment(5, 0);

            var (_, reward, isDone) = env.DebugStep(-1);

            Assert.AreEqual(0, reward);
            Assert.True(isDone);
        }

        [Test]
        public void WhenStartingAtRightmostPosition_StepRight_Wins()
        {
            var env = new RandomWalkEnvironment(5, 4);
        
            var (_, reward, isDone) = env.DebugStep(1);
        
            Assert.AreEqual(1, reward);
            Assert.True(isDone);
        }

        [Test]
        public void StartingAtInvalidState_Throws()
        {
            Assert.Throws<ArgumentException>(() => new RandomWalkEnvironment(5, 5));
        }

        [Test]
        public void WhenStartingInMiddle_StepRight_IncrementsPosition()
        {
            var env = new RandomWalkEnvironment(5, 3);

            var (state, reward, isDone) = env.DebugStep(1);

            Assert.AreEqual(4, state);
            Assert.AreEqual(0, reward);
            Assert.False(isDone);
        }

        [Test]
        public void CannotStep_WhenDone()
        {
            var env = new RandomWalkEnvironment(5, 4);
            env.DebugStep(1);

            // act & assert
            Assert.Throws<InvalidOperationException>(() => env.DebugStep(1));
            Assert.Throws<InvalidOperationException>(() => env.DebugStep(-1));
        }
    }
}