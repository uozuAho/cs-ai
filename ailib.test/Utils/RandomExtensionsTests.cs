using System;
using System.Linq;
using ailib.Utils;
using NUnit.Framework;

namespace ailib.test.Utils
{
    internal class RandomExtensionsTests
    {
        [Test]
        public void ChoiceOfOne_PicksOne()
        {
            var options = new[] {1};
            var random = new Random();

            Assert.AreEqual(1, random.Choice(options));
        }

        [Test]
        public void ChoiceOfTwo_PicksRandomly()
        {
            var options = new[] { 1, 2 };
            var random = new Random();

            var distinctChoices = Enumerable.Range(0, 20)
                .Select(_ => random.Choice(options))
                .Distinct();

            Assert.Greater(distinctChoices.Count(), 1);
        }
    }
}
