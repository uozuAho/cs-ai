using System;
using System.Collections.Generic;
using System.Linq;

namespace CliffWalking
{
    public record Position(int X, int Y);
    public record Step(Position Observation, double Reward, bool IsDone);

    /// <summary>
    /// An AI Gym-like interface for the "Cliff Walking" example in the RL
    /// book, example 6.6.
    /// See https://gym.openai.com/docs/
    /// </summary>
    public class CliffWalkingEnvironment
    {
        private Position _currentPosition = new(0, 0);

        public Step Step(CliffWalkingAction action)
        {
            Move(action);

            return new Step(_currentPosition, -1.0, false);
        }

        public IEnumerable<CliffWalkingAction> ActionSpace()
        {
            yield return CliffWalkingAction.Up;
            yield return CliffWalkingAction.Right;
        }

        private void Move(CliffWalkingAction action)
        {
            if (!ActionSpace().Contains(action))
                throw new InvalidOperationException("Invalid action");

            var (x, y) = _currentPosition;

            switch (action)
            {
                case CliffWalkingAction.Up: y += 1; break;
                case CliffWalkingAction.Right: x += 1; break;
                case CliffWalkingAction.Down: y -= 1; break;
                case CliffWalkingAction.Left: x -= 1; break;
                default:
                    throw new InvalidOperationException("invalid action");
            }

            _currentPosition = new Position(x, y);
        }
    }
}
