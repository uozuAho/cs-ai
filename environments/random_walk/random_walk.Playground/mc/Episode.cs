using System.Collections.Generic;

namespace random_walk.Playground.mc
{
    internal class Episode
    {
        public int Length => Steps.Count;
        public List<EpisodeStep> Steps { get; set; } = new();

        public static Episode Generate(RandomWalkEnvironment env)
        {
            var episode = new Episode();
            env.Reset();
            RandomWalkStepResult step;

            do
            {
                step = env.Step();
                episode.Steps.Add(new EpisodeStep(step.State, step.Reward));
            } while (!step.IsDone);

            return episode;
        }

        public int TimeOfFirstVisit(int state)
        {
            return Steps.FindIndex(step => step.State == state);
        }
    }
}