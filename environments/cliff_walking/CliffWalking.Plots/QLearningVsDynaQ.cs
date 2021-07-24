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

        public static void Run()
        {
            var learningRates = new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            var planningSteps = new[] {0, 1, 5};

            var agents = new[]
            {
                new QAgentResults
                {
                    Label = "Q learning",
                    CreateAgentFunc = (rate, steps) => new QLearningCliffWalker(Epsilon, rate),
                    // AsymptoticPerformance = new[]
                    // {
                    //     -49.55242,-49.09966,-49.05334,-49.07107,-49.10195,-49.133,-48.82859,-48.91728,-49.10077,-48.49301
                    // },
                    // InterimPerformance = new []
                    // {
                    //     -139.4942,-104.38739999999999,-92.9796,-84.31559999999998,-80.2604,-77.59,-77.22980000000001,-76.66980000000001,-76.05700000000002,-68.3978
                    // }
                },
                new QAgentResults
                {
                    Label = "Dyna Q",
                    CreateAgentFunc = (rate, steps) => new DynaQCliffWalker(Epsilon, rate, steps)
                }
            };

            // todo: plot q learning
            // todo: plot dyna q
            PlotAgents(agents, learningRates);
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
        /// <summary>
        /// learning rate, num planning steps -> agent
        /// </summary>
        public Func<double, int, ICliffWalkingAgent> CreateAgentFunc { get; set; }

        public double[] AsymptoticPerformance { get; set; } = { };
        public double[] InterimPerformance { get; set; } = { };
    }
}
