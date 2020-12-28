using System;
using random_walk.Playground.mc;
using random_walk.Playground.WPF;

namespace random_walk.Playground
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            EstimateStateValuesWithMonteCarlo();
        }

        /// <summary>
        /// Estimate the state values of a 5-state random walk, as per p148
        /// of the RL book. There is a reward of 0 for walking past the leftmost
        /// state, and 1 for the rightmost state.
        /// </summary>
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
    }
}
