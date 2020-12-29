using NUnit.Framework;

namespace random_walk.Test
{
    class RandomWalkEpisodeTests
    {
        [Test]
        public void asdf()
        {
            var env = new RandomWalkEnvironment(5);
            var episode = RandomWalkEpisode.Generate(env);
        }
    }
}
