using System;
using ailib.Utils;

namespace random_walk
{
    /// <summary>
    /// An N-state random walk, as per p148 of the RL book. There is a
    /// reward of 0 for walking past the leftmost  state, and 1 for
    /// the rightmost state.
    /// </summary>
    public class RandomWalkEnvironment
    {
        public int NumPositions { get; }

        private int _currentPosition;
        private readonly Random _random;

        private static readonly int[] Actions = {-1, 1};

        public RandomWalkEnvironment(int numPositions, int? startingPosition = null)
        {
            NumPositions = numPositions;
            _random = new Random();
            Reset(startingPosition);
        }

        public int Reset(int? startingPosition = null)
        {
            if (startingPosition < 0 || startingPosition >= NumPositions)
                throw new ArgumentException($"position must be between 0 and {NumPositions - 1}");

            _currentPosition = startingPosition ?? _random.Next(0, NumPositions);
            return _currentPosition;
        }

        public RandomWalkStepResult Step()
        {
            return DebugStep(_random.Choice(Actions));
        }

        /// <summary>
        /// Only used for testing. Don't use this!
        /// </summary>
        public RandomWalkStepResult DebugStep(int action)
        {
            if (IsDone) throw new InvalidOperationException("Cannot step when done");

            _currentPosition += action;
            var reward = _currentPosition >= NumPositions ? 1.0 : 0.0;

            return new RandomWalkStepResult(_currentPosition, reward, IsDone);
        }

        private bool IsDone => _currentPosition == -1 || _currentPosition >= NumPositions;
    }
}
