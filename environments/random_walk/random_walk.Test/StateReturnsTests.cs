using System;
using NUnit.Framework;

namespace random_walk.Test
{
    internal class StateReturnsTests
    {
        [Test]
        public void BadState_Throws()
        {
            var returns = new StateReturns(5);

            Assert.Throws<IndexOutOfRangeException>(() => returns.Add(-1, 1));
            Assert.Throws<IndexOutOfRangeException>(() => returns.Add(5, 1));
        }

        [Test]
        public void Averages_AreCorrect()
        {
            var returns = new StateReturns(5);

            returns.Add(4, 2);
            returns.Add(4, 4);

            Assert.AreEqual(3, returns.AverageReturnFrom(4));
        }
    }
}
