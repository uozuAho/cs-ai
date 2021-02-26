using System;
using System.Collections.Generic;
using System.Linq;
using CliffWalking.Agent;
using WpfPlotter;

namespace CliffWalking.Plots
{
    /// <summary>
    /// Plots interim vs asymptotic performance of a number of agents, similar to
    /// Figure 6.3, Expected Sarsa
    /// </summary>
    internal class InterimVsAsymptotic
    {
        public static void Run()
        {
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            // var asym = GatherAsymptoticPerformance(learningRates).ToArray();

            // gathering asymptotic data takes a while, saved it here:
            var asym = new[]
            {
                -24.71747, -25.29603, -25.96466, -27.26097, -28.95513, -33.26777, -40.56129, -52.7869, -86.58512,
                -1071.29097
            };

            PrintValues(asym);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Sum of rewards per episode vs learning rate");
            plt.PlotScatter(learningRates, asym, label: "TD 0 (Sarsa)");

            plt.XLabel("Learning rate");
            plt.YLabel("Sum");

            // same x axis range as book
            plt.Axis(0.1, 1.0, -160, 0);

            plt.Legend();

            plotter.Show();
        }

        private static IEnumerable<double> GatherAsymptoticPerformance(IEnumerable<double> learningRates)
        {
            const int numEpisodes = 100000;
            const double epsilon = 0.1;

            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                env.Reset();
                var agent = new Td0CliffWalker(epsilon, rate);

                agent.ImproveEstimates(env, out var diags, numEpisodes);

                yield return diags.RewardSumPerEpisode.Average();
            }
        }

        private static void PrintValues(IEnumerable<double> asym)
        {
            Console.WriteLine(string.Join(",", asym));
        }
    }
}
