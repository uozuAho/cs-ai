using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Agent.MonteCarlo
{
    internal class Episode
    {
        public List<EpisodeStep> Steps { get; private set; }

        public static Episode Generate(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            return new Episode {Steps = GenerateEpisode(agent, opponent).ToList()};
        }

        private static IEnumerable<EpisodeStep> GenerateEpisode(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            var env = new TicTacToeEnvironment(opponent);

            var board = env.Reset();
            var done = false;
            var lastReward = 0.0;

            while (!done)
            {
                var action = agent.GetAction(env, board);

                yield return new EpisodeStep
                {
                    Reward = lastReward,
                    State = board,
                    Action = action
                };

                var step = env.Step(action);

                board = step.Board;
                lastReward = step.Reward;
                done = step.IsDone;
            }

            yield return new EpisodeStep
            {
                Reward = lastReward,
                State = board,
                Action = null
            };
        }
    }
}