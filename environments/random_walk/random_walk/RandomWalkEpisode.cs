using System.Collections.Generic;

namespace random_walk
{
    public class RandomWalkEpisode
    {
        public int Length => Steps.Count;
        public List<RandomWalkStep> Steps { get; set; } = new();

        public static RandomWalkEpisode Generate(RandomWalkEnvironment env)
        {
            var episode = new RandomWalkEpisode();
            env.Reset();
            RandomWalkStepResult step;

            do
            {
                step = env.Step();
                episode.Steps.Add(new RandomWalkStep(step.State, step.Reward));
            } while (!step.IsDone);

            return episode;
        }

        public int TimeOfFirstVisit(int state)
        {
            return Steps.FindIndex(step => step.State == state);
        }
    }
}