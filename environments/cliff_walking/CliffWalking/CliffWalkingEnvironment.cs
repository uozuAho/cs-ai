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
        private static readonly Position BottomLeft = new(0, 0);
        private static readonly Position DefaultStartingPosition = BottomLeft;
        private static readonly Position BottomRight = new(11, 0);
        private static readonly Position GoalPosition = BottomRight;

        private static readonly CliffWalkingAction[] AllActions =
            (CliffWalkingAction[]) Enum.GetValues(typeof(CliffWalkingAction));

        private Position _currentPosition;

        public CliffWalkingEnvironment()
        {
            Reset();
        }

        public CliffWalkingEnvironment(Position startingPosition)
        {
            if (startingPosition == GoalPosition)
                throw new ArgumentException("Cannot start at goal");

            _currentPosition = startingPosition;
        }

        public Position Reset()
        {
            _currentPosition = DefaultStartingPosition;
            return _currentPosition;
        }

        public Step Step(CliffWalkingAction action)
        {
            Move(action);
            var reward = Reward();
            if (IsOnCliff())
                _currentPosition = DefaultStartingPosition;

            return new Step(_currentPosition, reward, _currentPosition == GoalPosition);
        }

        public IEnumerable<CliffWalkingAction> ActionSpace()
        {
            var actions = AllActions.Select(a => a).ToList();

            if (_currentPosition.X == 0)
                actions.Remove(CliffWalkingAction.Left);
            if (_currentPosition.X == 11)
                actions.Remove(CliffWalkingAction.Right);
            if (_currentPosition.Y == 0)
                actions.Remove(CliffWalkingAction.Down);
            if (_currentPosition.Y == 3)
                actions.Remove(CliffWalkingAction.Up);

            return actions;
        }

        private double Reward()
        {
            if (IsOnCliff())
            {
                return -100;
            }

            return -1;
        }

        private bool IsOnCliff()
        {
            return _currentPosition.Y == 0
                   && _currentPosition.X > 0
                   && _currentPosition.X < 11;
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
