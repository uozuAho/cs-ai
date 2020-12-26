using System;

namespace random_walk
{
    public class RandomWalkEnvironment
    {
        private readonly int _numPositions;
        private int _currentPosition;

        public RandomWalkEnvironment(int numPositions, int startingPosition)
        {
            _numPositions = numPositions;
            _currentPosition = startingPosition;
        }

        public RandomWalkStepResult Step(int action)
        {
            if (IsDone) throw new InvalidOperationException("Cannot step when done");

            _currentPosition += action;
            var reward = _currentPosition > _numPositions ? 1.0 : 0.0;

            return new RandomWalkStepResult(_currentPosition, reward, IsDone);
        }

        private bool IsDone => _currentPosition == -1 || _currentPosition > _numPositions;
    }

    public record RandomWalkStepResult(int State, double Reward, bool IsDone);
}
