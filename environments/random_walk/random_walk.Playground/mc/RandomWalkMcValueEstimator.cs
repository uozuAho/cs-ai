using System.Linq;

namespace random_walk.Playground.mc
{
    internal class RandomWalkMcValueEstimator
    {
        private double[] _values;
        private StateReturns _returns;

        public double[] Estimate(RandomWalkEnvironment environment, int? episodeLimit = null)
        {
            _values = new double[environment.NumPositions];
            _returns = new StateReturns(environment.NumPositions);

            var maxEpisodes = episodeLimit ?? 10000;

            for (var i = 0; i < maxEpisodes; i++)
            {
                ImproveEstimates();
            }

            return _values;
        }

        private void ImproveEstimates()
        {
            var rewardSum = 0.0;
            var episode = RandomWalkEpisode.Generate(new RandomWalkEnvironment(5));

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