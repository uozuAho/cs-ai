using System;
using System.Collections;
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
        public (int, int) Dimensions => (12, 4);
        public Position CurrentPosition;

        private static readonly Position BottomLeft = new(0, 0);
        private static readonly Position DefaultStartingPosition = BottomLeft;
        private static readonly Position BottomRight = new(11, 0);
        private static readonly Position GoalPosition = BottomRight;

        public CliffWalkingEnvironment() : this(DefaultStartingPosition)
        {
        }

        public CliffWalkingEnvironment(Position startingPosition)
        {
            if (startingPosition == GoalPosition)
                throw new ArgumentException("Cannot start at goal");

            CurrentPosition = startingPosition;
        }

        public Position Reset()
        {
            CurrentPosition = DefaultStartingPosition;
            return CurrentPosition;
        }

        public Step Step(CliffWalkingAction action)
        {
            Move(action);
            var reward = Reward();
            if (IsOnCliff())
                CurrentPosition = DefaultStartingPosition;

            return new Step(CurrentPosition, reward, CurrentPosition == GoalPosition);
        }

        public IEnumerable<CliffWalkingAction> ActionSpace()
        {
            return ActionSpace(CurrentPosition);
        }

        public static IEnumerable<CliffWalkingAction> ActionSpace(Position position)
        {
            var (x, y) = position;

            if (x != 0)
                yield return CliffWalkingAction.Left;
            if (x != 11)
                yield return CliffWalkingAction.Right;
            if (y != 0)
                yield return CliffWalkingAction.Down;
            if (y != 3)
                yield return CliffWalkingAction.Up;
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
            return CurrentPosition.Y == 0
                   && CurrentPosition.X > 0
                   && CurrentPosition.X < 11;
        }

        private void Move(CliffWalkingAction action)
        {
            if (!ActionSpace().Contains(action))
                throw new InvalidOperationException("Invalid action");

            var (x, y) = CurrentPosition;

            switch (action)
            {
                case CliffWalkingAction.Up: y += 1; break;
                case CliffWalkingAction.Right: x += 1; break;
                case CliffWalkingAction.Down: y -= 1; break;
                case CliffWalkingAction.Left: x -= 1; break;
                default:
                    throw new InvalidOperationException("invalid action");
            }

            CurrentPosition = new Position(x, y);
        }
    }
}
