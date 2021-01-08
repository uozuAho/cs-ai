using System.Collections.Generic;

namespace CliffWalking
{
    /// <summary>
    /// An AI Gym-like interface for the "Cliff Walking" example in the RL
    /// book, example 6.6.
    /// See https://gym.openai.com/docs/
    /// </summary>
    public class CliffWalkingEnvironment
    {
        public Step Step(CliffWalkingAction action)
        {
            return new(new Position(1, 2), 1.0, false);
        }

        public IEnumerable<CliffWalkingAction> ActionSpace()
        {
            yield return CliffWalkingAction.Up;
        }
    }

    public record Step(Position Observation, double Reward, bool IsDone);

    public record Position(int X, int Y);
}
