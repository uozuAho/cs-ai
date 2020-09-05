using System;
using NUnit.Framework;

namespace dp.test
{
    public class ValueTableTests
    {
        [SetUp]
        public void Setup()
        {
        }

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

            foreach (var state in gamblersWorld.AllStates())
            {
                var genericValue = genericValues.Value(state);
                var gamblerValue = genericValues.Value(state);

                if (Math.Abs(genericValue - genericValue) > double.Epsilon)
                {
                    throw new Exception($"values not equal for state {state}. " +
                                        $"generic: {genericValue}, gambler: {gamblerValue}");
                }
            }

            Console.WriteLine("PASS: All values match!");
        }
    }
}
