using System;
using dp.GamblersProblem;
using NUnit.Framework;

namespace dp.test
{
    public class ValueTableTests
    {
        [Test]
        public void Evaluates_to_same_values_as_gamblers_value_table()
        {
            const double probabilityOfHeads = 0.4;
            const int dollarsToWin = 100;

            var gamblersWorld = new GamblersWorld(probabilityOfHeads, dollarsToWin);
            var rewarder = new GamblersWorldRewarder(gamblersWorld);
            var policy = new UniformRandomGamblersPolicy();

            var gamblersValues = new GamblersValueTable(gamblersWorld);
            var genericValues = new ValueTable<GamblersWorldState, GamblersWorldAction>(gamblersWorld);

            gamblersValues.Evaluate(policy, rewarder);
            genericValues.Evaluate(policy, rewarder);

            Assert.That(() => AllValuesAreEqual(gamblersWorld, genericValues, gamblersValues));
        }

        private static bool AllValuesAreEqual(
            GamblersWorld world,
            ValueTable<GamblersWorldState, GamblersWorldAction> genericValues,
            GamblersValueTable gamblersValues)
        {
            foreach (var state in world.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gamblerValue = genericValues.Value(state);

                Assert.AreEqual(genericValue, gamblerValue, double.Epsilon,
                    $"values not equal for state {state}. " +
                    $"generic: {genericValue}, gambler: {gamblerValue}");
            }

            return true;
        }
    }
}
