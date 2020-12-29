using System.Linq;

namespace random_walk.Playground.mc
{
    internal class Td0ValueEstimator
    {
        private double[] _values;
        private readonly double _learningRate;

        public Td0ValueEstimator(double learningRate = 0.1)
        {
            _learningRate = learningRate;
        }

        public double[] Estimate(RandomWalkEnvironment environment, int? episodeLimit = null)
        {
            _values = Enumerable.Range(0, environment.NumPositions).Select(_ => 0.5).ToArray();

            var maxEpisodes = episodeLimit ?? 10000;

            for (var i = 0; i < maxEpisodes; i++)
            {
                var state = environment.Reset();
                bool done;

                do
                {
                    var (nextState, reward, isDone) = environment.Step();
                    done = isDone;
                    _values[state] = Value(state) +
                                     _learningRate * (reward + Value(nextState) - Value(state));
                    state = nextState;
                } while (!done);
            }

            return _values;
        }

        private double Value(int state)
        {
            if (state < 0 || state >= _values.Length) return 0;

            return _values[state];
        }
    }
}
