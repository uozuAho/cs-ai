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
    internal class QLearningVsDynaQ
    {
        private const int NumEpisodesForAsymptote = 100000;
        private const int NumEpisodesForInterim = 100;
        private const double Epsilon = 0.1; // chance of random action, 'e' in e-greedy

        private static ICliffWalkingAgent CreateQLearner(double rate)
            => new QLearningCliffWalker(Epsilon, rate);

        private static ICliffWalkingAgent CreateDynaQAgent(double rate, int planningSteps)
            => new DynaQCliffWalker(Epsilon, rate, planningSteps);

        public static void Run()
        {
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

            var qLearner = new QAgentResults
            {
                Label = "Q learning",
                InterimPerformance = new[]
                {
                    -139.76239999999999, -104.042, -88.63319999999997, -85.18440000000001, -81.70179999999999,
                    -79.67020000000001, -76.99619999999999, -74.72879999999998, -75.48160000000001, -70.7964
                },
                AsymptoticPerformance = new[]
                {
                    -49.66553, -49.32863, -49.11585, -48.82648, -49.15973, -49.12489, -48.8334, -48.94537, -48.78564,
                    -49.6059
                }
            };

            Console.WriteLine("Gathering interim performance for dynaq zero");

            var dynaQZeroSteps = new QAgentResults
            {
                Label = "DynaQ Zero",
                InterimPerformance = GatherInterimPerformance(learningRates, rate => CreateDynaQAgent(rate, 0)).ToArray()
            };

            Console.WriteLine("Gathering interim performance for dynaq 3");

            var dynaQ3Steps = new QAgentResults
            {
                Label = "DynaQ 3",
                InterimPerformance = GatherInterimPerformance(learningRates, rate => CreateDynaQAgent(rate, 3)).ToArray()
            };

            Console.WriteLine("Gathering interim performance for dynaq 10");

            var dynaQ10Steps = new QAgentResults
            {
                Label = "DynaQ 10",
                InterimPerformance = GatherInterimPerformance(learningRates, rate => CreateDynaQAgent(rate, 3)).ToArray()
            };

            // Console.WriteLine("qlearner");
            // PrintValues(qLearner.InterimPerformance);
            // PrintValues(qLearner.AsymptoticPerformance);

            var agents = new[] {qLearner, dynaQZeroSteps, dynaQ3Steps, dynaQ10Steps};

            PlotAgents(agents, learningRates);
        }

        private static IEnumerable<double> GatherInterimPerformance(
            IEnumerable<double> learningRates,
            Func<double, ICliffWalkingAgent> createAgentFunc)
        {
            const int numRuns = 50;

            var env = new CliffWalkingEnvironment();

            foreach (var rate in learningRates)
            {
                Console.WriteLine($"learning rate: {rate}");
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

        private static void PlotAgents(IEnumerable<QAgentResults> agents, double[] learningRates)
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

    internal class QAgentResults
    {
        public string Label { get; set; }
        public double[] AsymptoticPerformance { get; set; } = { };
        public double[] InterimPerformance { get; set; } = { };
    }
}
