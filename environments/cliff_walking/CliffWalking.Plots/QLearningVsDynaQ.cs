using System;
using System.Collections.Generic;
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

        public static void Run()
        {
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            var planningSteps = new[] {0, 1, 5};

            var qLearner = new QAgentResults 
            {
                Label = "Q learning",
                InterimPerformance = GatherInterimPerformance(learningRates, CreateQLearner).ToArray()
            };

            var agents = new[] {qLearner};

            // todo: plot q learning
            // todo: plot dyna q
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
    }

    internal class QAgentResults
    {
        public string Label { get; set; }
        public double[] AsymptoticPerformance { get; set; } = { };
        public double[] InterimPerformance { get; set; } = { };
    }
}
