using System.Collections;
using System.Linq;
using dp.Examples.GamblersProblem;
using NUnit.Framework;

namespace dp.test
{
    class GamblersWorldTests
    {
        [Test]
        public void Has_states_equal_to_dollars_to_win_plus_one(
            [Range(1, 100)] int dollarsToWin)
        {
            const double probabilityOfHeads = 0.4;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);

            Assert.AreEqual(dollarsToWin + 1, world.AllStates().Count());
        }

        [TestCase(1, 0, 0)]
        [TestCase(10, 0, 0)]
        [TestCase(10, 1, 1)]
        [TestCase(10, 2, 2)]
        [TestCase(10, 8, 2)]
        [TestCase(10, 9, 1)]
        [TestCase(10, 10, 0)]
        public void Number_of_possible_actions(int dollarsToWin, int dollarsInHand, int numPossibleActions)
        {
            const double probabilityOfHeads = 0.4;

            var world = new GamblersWorld(probabilityOfHeads, dollarsToWin);

            var actualNumberOfActions = world
                .AvailableActions(new GamblersWorldState(dollarsInHand))
                .Count();

            Assert.AreEqual(numPossibleActions, actualNumberOfActions,
                $"expected {numPossibleActions} possible actions when ${dollarsToWin} to win, " +
                $"and ${dollarsInHand} in hand. Got {actualNumberOfActions}");
        }

    }

    class GamblersWorldWith100DollarsToWinData
    {
        public static IEnumerable Data
        {
            get
            {
                for (var i = 1; i < 100; i++)
                {
                    yield return new TestFixtureData(i, 1);
                }
            }
        }
    }

    [TestFixtureSource(typeof(GamblersWorldWith100DollarsToWinData), "Data")]
    class GamblersWorldWith100DollarsToWin
    {
        private const double ProbabilityOfHeads = 0.4;
        private readonly GamblersWorld _world;
        private readonly GamblersWorldState _currentState;
        private readonly GamblersWorldAction _action;

        public GamblersWorldWith100DollarsToWin(int dollarsInHand, int stake)
        {
            _currentState = new GamblersWorldState(dollarsInHand);
            _action = new GamblersWorldAction(stake);

            _world = new GamblersWorld(ProbabilityOfHeads, 100);
        }

        [Test]
        public void Any_action_has_two_possible_outcomes()
        {
            Assert.AreEqual(2, _world.PossibleStates(_currentState, _action).Count());
        }

        [Test]
        public void Probability_of_possible_outcomes_sums_to_1()
        {
            var sumOfProbabilities = _world.PossibleStates(_currentState, _action).Sum(s => s.Item2);

            Assert.AreEqual(1, sumOfProbabilities);
        }

        [Test]
        public void Probability_of_losing_each_flip_is_constant()
        {
            var lossOutcome = _world
                .PossibleStates(_currentState, _action)
                .Single(s => s.Item1.DollarsInHand == _currentState.DollarsInHand - 1);

            Assert.AreEqual(1 - ProbabilityOfHeads, lossOutcome.Item2);
        }
    }
}
