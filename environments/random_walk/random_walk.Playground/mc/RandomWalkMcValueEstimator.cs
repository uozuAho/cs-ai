using System.Linq;
using random_walk.Playground.WPF;

namespace random_walk.Playground.mc
{
    internal class RandomWalkMcValueEstimator
    {
        private double[] _values;
        private StateReturns _returns;

        public static void Run()
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

            var mcEstimator = new RandomWalkMcValueEstimator();
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

        public double[] Estimate(RandomWalkEnvironment environment, int? episodeLimit = null)
        {
            _values = new double[environment.NumPositions];
            _returns = new StateReturns(environment.NumPositions);

            var maxEpisodes = episodeLimit ?? 10000;

            for (var i = 0; i < maxEpisodes; i++)
            {
                ImproveEstimates(environment);
            }

            return _values;
        }

        private void ImproveEstimates(RandomWalkEnvironment environment)
        {
            var rewardSum = 0.0;
            var episode = RandomWalkEpisode.Generate(environment);

            foreach (var t in Enumerable.Range(0, episode.Length - 1).Reverse())
            {
                var state = episode.Steps[t].State;
                rewardSum += episode.Steps[t + 1].Reward;

                if (episode.TimeOfFirstVisit(state) == t)
                {
                    _returns.Add(state, rewardSum);
                    _values[state] = _returns.AverageReturnFrom(state);
                }
            }
        }
    }
}