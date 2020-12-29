using System;
using System.Collections.Generic;
using System.Linq;
using random_walk.Playground.mc;
using random_walk.Playground.WPF;

namespace random_walk.Playground
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // EstimateStateValuesWithMonteCarlo();
            // EstimateStateValuesWithTd0();
            CompareMcAndTd0();
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

                    mcErrors[j] = RmsError(actualValues, mcEstimates);
                    tdErrors05[j] = RmsError(actualValues, tdEstimates05);
                    tdErrors10[j] = RmsError(actualValues, tdEstimates10);
                    tdErrors20[j] = RmsError(actualValues, tdEstimates20);
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
            plt.PlotScatter(dataX, avgTdErrors05, label: "td 0.05");
            plt.PlotScatter(dataX, avgTdErrors10, label: "td 0.10");
            plt.PlotScatter(dataX, avgTdErrors20, label: "td 0.20");

            plt.Legend();

            plotter.Show();
        }

        private static double RmsError(IEnumerable<double> actual, IEnumerable<double> estimate)
        {
            var meanSquare = actual.Zip(estimate)
                .Select(x => Math.Pow(x.First - x.Second, 2))
                .Average();

            return Math.Sqrt(meanSquare);
        }
    }
}
