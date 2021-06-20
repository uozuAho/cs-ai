﻿using System;
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

            // todo: create separate methods to get performance for q and dyna : q doesn't care about planning steps
            GatherAsymptoticPerformance(agents, learningRates);
            GatherInterimPerformance(agents, learningRates);
            PlotAgents(agents, learningRates);
        }

        private static void GatherAsymptoticPerformance(
            IEnumerable<QAgentResults> agents,
            double[] learningRates,
            int[] planningSteps)
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
            Func<double, int, ICliffWalkingAgent> createAgentFunc)
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

    internal class QAgentResults
    {
        public string Label { get; set; }
        /// <summary>
        /// learning rate, num planning steps -> agent
        /// </summary>
        public Func<double, int, ICliffWalkingAgent> CreateAgentFunc { get; set; }
        public double[] AsymptoticPerformance { get; set; }
        public double[] InterimPerformance { get; set; }
    }
}
