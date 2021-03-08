using System;
using System.Collections.Generic;
using System.Linq;
using random_walk.Playground.mc;
using ScottPlot;
using WpfPlotter;

namespace random_walk.Playground
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // EstimateStateValuesWithMonteCarlo();
            // EstimateStateValuesWithTd0();
            // CompareMcAndTd0();
            CompareNStepLengths();
        }

        public static void EstimateStateValuesWithMonteCarlo()
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

            var mcEstimator = new McValueEstimator();
            var mcEstimates10 = mcEstimator.Estimate(env, 10);
            var mcEstimates100 = mcEstimator.Estimate(env, 100);
            var mcEstimates1000 = mcEstimator.Estimate(env, 1000);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("MC random walk estimates after X episodes");
            double[] dataX = { 1, 2, 3, 4, 5 };
            plt.PlotScatter(dataX, actualValues, label: "actual");
            plt.PlotScatter(dataX, mcEstimates10, label: "10");
            plt.PlotScatter(dataX, mcEstimates100, label: "100");
            plt.PlotScatter(dataX, mcEstimates1000, label: "1000");

            plt.Legend();

            plotter.Show();
        }

        public static void EstimateStateValuesWithTd0()
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

            var estimator = new Td0ValueEstimator();
            var estimates10 = estimator.Estimate(env, 10);
            var estimates100 = estimator.Estimate(env, 100);
            var estimates1000 = estimator.Estimate(env, 1000);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("TD0 random walk estimates after X episodes");
            double[] dataX = { 1, 2, 3, 4, 5 };
            plt.PlotScatter(dataX, actualValues, label: "actual");
            plt.PlotScatter(dataX, estimates10, label: "10");
            plt.PlotScatter(dataX, estimates100, label: "100");
            plt.PlotScatter(dataX, estimates1000, label: "1000");

            plt.Legend();

            plotter.Show();
        }

        private static void CompareMcAndTd0()
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

            var mcEstimator = new McValueEstimator();
            var td0ValueEstimator05 = new Td0ValueEstimator(0.05);
            var td0ValueEstimator10 = new Td0ValueEstimator(0.10);
            var td0ValueEstimator20 = new Td0ValueEstimator(0.20);

            var avgMcErrors = new double[100];
            var avgTdErrors05 = new double[100];
            var avgTdErrors10 = new double[100];
            var avgTdErrors20 = new double[100];

            for (var i = 0; i < 100; i++)
            {
                var mcErrors = new double[100];
                var tdErrors05 = new double[100];
                var tdErrors10 = new double[100];
                var tdErrors20 = new double[100];

                for (var j = 0; j < 100; j++)
                {
                    var mcEstimates = mcEstimator.Estimate(env, i);
                    var tdEstimates05 = td0ValueEstimator05.Estimate(env, i);
                    var tdEstimates10 = td0ValueEstimator10.Estimate(env, i);
                    var tdEstimates20 = td0ValueEstimator20.Estimate(env, i);

                    mcErrors[j] = AvgRmsError(actualValues, mcEstimates);
                    tdErrors05[j] = AvgRmsError(actualValues, tdEstimates05);
                    tdErrors10[j] = AvgRmsError(actualValues, tdEstimates10);
                    tdErrors20[j] = AvgRmsError(actualValues, tdEstimates20);
                }

                avgMcErrors[i] = mcErrors.Average();
                avgTdErrors05[i] = tdErrors05.Average();
                avgTdErrors10[i] = tdErrors10.Average();
                avgTdErrors20[i] = tdErrors20.Average();
            }

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("MC and TD0 RMS error vs num episodes");
            var dataX = Enumerable.Range(0, 100).Select(x => (double)x).ToArray();
            plt.PlotScatter(dataX, avgMcErrors, label: "mc");
            plt.PlotScatter(dataX, avgTdErrors05, label: "td, learning rate: 0.05");
            plt.PlotScatter(dataX, avgTdErrors10, label: "td, learning rate: 0.10");
            plt.PlotScatter(dataX, avgTdErrors20, label: "td, learning rate: 0.20");

            plt.Legend(location: legendLocation.upperRight);

            plotter.Show();
        }

        private static void CompareNStepLengths()
        {
            var env = new RandomWalkEnvironment(19, 10);
            // actual probability of reaching goal state from given state
            var actualValues = Enumerable.Range(1, 19).Select(i => i / 20.0).ToArray();
            var learningRates = new[] {0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0};

            var nStepResults = new[] {1, 2, 4}
                .Select(numSteps => new NStepResult
                {
                    Label = $"{numSteps}",
                    CreateEstimatorFunc = learningRate => new NStepEstimator(learningRate, numSteps)
                })
                .ToList();

            foreach (var nStepResult in nStepResults)
            {
                foreach (var learningRate in learningRates)
                {
                    var avgRmsErrorSum = 0.0;
                    const int numRuns = 20;
                    for (var i = 0; i < numRuns; i++)
                    {
                        var estimator = nStepResult.CreateEstimatorFunc(learningRate);
                        var estimates = estimator.Estimate(env, 10);
                        var avgError = AvgRmsError(actualValues, estimates);
                        avgRmsErrorSum += avgError;
                    }
                    nStepResult.RmsErrors.Add(avgRmsErrorSum / numRuns);
                }
            }

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Average RMS error over 19 states and first 10 episodes");
            plt.XLabel("Learning rate");
            plt.YLabel("Avg. RMS error");
            foreach (var nStepResult in nStepResults)
            {
                plt.PlotScatter(learningRates, nStepResult.RmsErrors.ToArray(), label: nStepResult.Label);
            }
            plt.Legend();
            plotter.Show();
        }

        private class NStepResult
        {
            public string Label { get; init; }
            public Func<double, NStepEstimator> CreateEstimatorFunc { get; init; }
            public List<double> RmsErrors { get; } = new();
        }

        private static double AvgRmsError(IEnumerable<double> actual, IEnumerable<double> estimate)
        {
            var meanSquare = actual.Zip(estimate)
                .Select(x => Math.Pow(x.First - x.Second, 2))
                .Average();

            return Math.Sqrt(meanSquare);
        }
    }
}
