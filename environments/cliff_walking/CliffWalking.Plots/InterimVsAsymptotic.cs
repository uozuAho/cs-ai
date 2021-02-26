using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CliffWalking.Agent;
using ScottPlot;
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
            const double epsilon = 0.1;
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

            var agents = new[]
            {
                new AgentResults
                {
                    Label = "TD 0 (Sarsa)",
                    CreateAgentFunc = rate => new Td0CliffWalker(epsilon, rate),
                    // comment this out to recalculate
                    AsymptoticPerformance = new[]
                    {
                        -24.71747, -25.29603, -25.96466, -27.26097, -28.95513, -33.26777, -40.56129, -52.7869, -86.58512, -1071.29097
                    }
                },
                new AgentResults
                {
                    Label = "Q learning",
                    CreateAgentFunc = rate => new QLearningCliffWalker(epsilon, rate),
                    // comment this out to recalculate
                    AsymptoticPerformance = new[]
                    {
                        -49.55242,-49.09966,-49.05334,-49.07107,-49.10195,-49.133,-48.82859,-48.91728,-49.10077,-48.49301
                    }
                }
            };

            GatherAsymptoticPerformance(agents, learningRates);
            GatherInterimPerformance(agents, learningRates);
            PlotAgents(agents, learningRates);
        }

        private static void GatherAsymptoticPerformance(IEnumerable<AgentResults> agents, double[] learningRates)
        {
            foreach (var agent in agents)
            {
                // use stored results if possible, since this takes a while
                if (agent.AsymptoticPerformance != null) continue;

                agent.AsymptoticPerformance = GatherAsymptoticPerformance(learningRates, agent.CreateAgentFunc).ToArray();
                Console.WriteLine(agent.Label);
                PrintValues(agent.AsymptoticPerformance);
            }
        }

        private static IEnumerable<double> GatherAsymptoticPerformance(
            IEnumerable<double> learningRates,
            Func<double, ICliffWalkingAgent> createAgentFunc)
        {
            const int numEpisodes = 100000;

            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                env.Reset();
                var agent = createAgentFunc(rate);

                agent.ImproveEstimates(env, out var diags, numEpisodes);

                yield return diags.RewardSumPerEpisode.Average();
            }
        }

        private static void GatherInterimPerformance(IEnumerable<AgentResults> agents, double[] learningRates)
        {
            foreach (var agent in agents)
            {
                agent.InterimPerformance = GatherInterimPerformance(learningRates, agent.CreateAgentFunc).ToArray();
            }
        }

        private static IEnumerable<double> GatherInterimPerformance(
            IEnumerable<double> learningRates,
            Func<double, ICliffWalkingAgent> createAgentFunc)
        {
            const int numRuns = 50;
            const int numEpisodesPerRun = 100;

            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                var firstXEpisodeAverages = new List<double>();

                for (var i = 0; i < numRuns; i++)
                {
                    env.Reset();
                    var agent = createAgentFunc(rate);

                    agent.ImproveEstimates(env, out var diags, numEpisodesPerRun);

                    firstXEpisodeAverages.Add(diags.RewardSumPerEpisode.Average());
                }

                yield return firstXEpisodeAverages.Average();
            }
        }

        private static void PlotAgents(IEnumerable<AgentResults> agents, double[] learningRates)
        {
            var plotter = new Plotter();
            var plt = plotter.Plt;
            var colors = new[]
            {
                Color.Black,
                Color.Blue,
                Color.BlueViolet,
                Color.Crimson,
            };

            plt.Title("Sum of rewards per episode vs learning rate");

            foreach (var (agent, color) in agents.Zip(colors))
            {
                plt.PlotScatter(learningRates,
                    agent.AsymptoticPerformance,
                    label: agent.Label + " asym",
                    color: color,
                    markerShape: MarkerShape.cross);

                plt.PlotScatter(learningRates,
                    agent.InterimPerformance,
                    label: agent.Label + " interim",
                    color: color,
                    markerShape: MarkerShape.filledCircle);
            }

            plt.XLabel("Learning rate");
            plt.YLabel("Sum");

            // same x axis range as book
            plt.Axis(0.1, 1.0, -160, 0);

            plt.Legend();

            plotter.Show();
        }

        private static void PrintValues(IEnumerable<double> asym)
        {
            Console.WriteLine(string.Join(",", asym));
        }
    }

    internal class AgentResults
    {
        public string Label { get; set; }
        public Func<double, ICliffWalkingAgent> CreateAgentFunc { get; set; }
        public double[] AsymptoticPerformance { get; set; }
        public double[] InterimPerformance { get; set; }
    }
}
