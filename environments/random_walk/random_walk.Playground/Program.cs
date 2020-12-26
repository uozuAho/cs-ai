using System;
using System.Collections.Generic;
using System.Linq;
using random_walk.Playground.mc;

namespace random_walk.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new RandomWalkEnvironment(5, 3);

            var actualValues = new[]
            {
                1.0 / 6,
                2.0 / 6,
                3.0 / 6,
                4.0 / 6,
                5.0 / 6
            };

            var mcEstimator = new RandomWalkMcValueEstimator();
            var mcEstimates10 = mcEstimator.Estimate(env, 10);
            var mcEstimates100 = mcEstimator.Estimate(env, 100);
            var mcEstimates1000 = mcEstimator.Estimate(env, 1000);

            PrintValues(mcEstimates10);
            PrintValues(mcEstimates100);
            PrintValues(mcEstimates1000);
            PrintValues(actualValues);
        }

        private static void PrintValues(IEnumerable<double> estimates)
        {
            var formattedEstimates = estimates.Select(e => $"{e:F}");
            Console.WriteLine($"estimates: {string.Join(",", formattedEstimates)}");
        }
    }
}
