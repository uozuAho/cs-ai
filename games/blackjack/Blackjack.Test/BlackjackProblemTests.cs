using NUnit.Framework;

namespace Blackjack.Test
{
    public class BlackjackProblemTests
    {
        private BlackjackProblem _problem;

        [SetUp]
        public void Setup()
        {
            _problem = new BlackjackProblem();
        }

        [TestCase(new[] { 10, 10, 10 })]
        [TestCase(new[] { 10, 10, 2 })]
        [TestCase(new[] { 10, 9, 1, 1, 1 })]
        public void player_is_bust(int[] playerCards)
        {
            var state = new BlackjackState {PlayerCards = playerCards};

            Assert.True(_problem.IsPlayerBust(state));
        }

        [TestCase(new[] { 10, 10, 1 })]
        [TestCase(new[] { 10, 9, 1, 1 })]
        [TestCase(new[] { 1, 1, 1, 1})]
        public void player_is_not_bust(int[] playerCards)
        {
            var state = new BlackjackState { PlayerCards = playerCards };

            Assert.False(_problem.IsPlayerBust(state));
        }

        [TestCase(new[] { 1 }, 11)]
        [TestCase(new[] { 2 }, 2)]
        [TestCase(new[] { 1, 1 }, 12)]
        [TestCase(new[] { 1, 2 }, 13)]
        [TestCase(new[] { 10, 1 }, 21)]
        [TestCase(new[] { 10, 2 }, 12)]
        [TestCase(new[] { 1, 1, 1 }, 13)]
        public void Sum(int[] cards, int expectedSum)
        {
            Assert.AreEqual(expectedSum, _problem.Sum(cards));
        }
    }
}