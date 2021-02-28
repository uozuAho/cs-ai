using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const int NumEpisodesForAsymptote = 100000;
        private const int NumEpisodesForInterim = 100;
        private const double Epsilon = 0.1; // chance of random action, 'e' in e-greedy

        public static void Run()
        {
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

            var agents = new[]
            {
                new AgentResults
                {
                    Label = "Sarsa",
                    CreateAgentFunc = rate => new SarsaCliffWalker(Epsilon, rate),
                    // Comment out the following performance data to recalculate.
                    // Values from previous runs add here to save time.
                    AsymptoticPerformance = new[]
                    {
                        -24.71747, -25.29603, -25.96466, -27.26097, -28.95513, -33.26777, -40.56129, -52.7869, -86.58512, -1071.29097
                    },
                    InterimPerformance = new []
                    {
                        -129.0826,-92.76539999999999,-79.0146,-73.6626,-70.15160000000002,-67.23040000000003,-70.0704,-68.117,-66.73240000000001,-61.22659999999999
                    }
                },
                new AgentResults
                {
                    Label = "Q learning",
                    CreateAgentFunc = rate => new QLearningCliffWalker(Epsilon, rate),
                    AsymptoticPerformance = new[]
                    {
                        -49.55242,-49.09966,-49.05334,-49.07107,-49.10195,-49.133,-48.82859,-48.91728,-49.10077,-48.49301
                    },
                    InterimPerformance = new []
                    {
                        -139.4942,-104.38739999999999,-92.9796,-84.31559999999998,-80.2604,-77.59,-77.22980000000001,-76.66980000000001,-76.05700000000002,-68.3978
                    }
                },
                new AgentResults
                {
                    Label = "Expected Sarsa",
                    CreateAgentFunc = rate => new ExpectedSarsaCliffWalker(Epsilon, rate),
                    AsymptoticPerformance = new []
                    {
                        -23.41282,-23.52287,-23.54294,-23.5803,-23.31136,-23.347,-23.6232,-23.41913,-23.38462,-23.50039
                    },
                    InterimPerformance = new []
                    {
                        -120.62039999999998,-84.183,-67.90859999999999,-59.79600000000001,-55.070999999999984,-51.64760000000001,-48.913799999999995,-47.46840000000001,-46.6214,-45.198199999999986
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

                agent.AsymptoticPerformance = GatherAsymptoticPerformance(
                    learningRates, agent.CreateAgentFunc).ToArray();
                Console.WriteLine(agent.Label);
                PrintValues(agent.AsymptoticPerformance);
            }
        }

        private static IEnumerable<double> GatherAsymptoticPerformance(
            IEnumerable<double> learningRates,
            Func<double, ICliffWalkingAgent> createAgentFunc)
        {
            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                env.Reset();
                var agent = createAgentFunc(rate);
                var sw = Stopwatch.StartNew();

                agent.ImproveEstimates(env, out var diags, NumEpisodesForAsymptote);

                Console.WriteLine($"ran {NumEpisodesForAsymptote} episodes in {sw.Elapsed}");

                yield return diags.RewardSumPerEpisode.Average();
            }
        }

        private static void GatherInterimPerformance(IEnumerable<AgentResults> agents, double[] learningRates)
        {
            foreach (var agent in agents)
            {
                // use stored results if possible, since this takes a while
                if (agent.InterimPerformance != null) continue;

                agent.InterimPerformance = GatherInterimPerformance(
                    learningRates, agent.CreateAgentFunc).ToArray();
                Console.WriteLine(agent.Label);
                PrintValues(agent.InterimPerformance);
            }
        }

        private static IEnumerable<double> GatherInterimPerformance(
            IEnumerable<double> learningRates,
            Func<double, ICliffWalkingAgent> createAgentFunc)
        {
            const int numRuns = 50;

            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                var firstXEpisodeAverages = new List<double>();

                for (var i = 0; i < numRuns; i++)
                {
                    env.Reset();
                    var agent = createAgentFunc(rate);

                    agent.ImproveEstimates(env, out var diags, NumEpisodesForInterim);

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
