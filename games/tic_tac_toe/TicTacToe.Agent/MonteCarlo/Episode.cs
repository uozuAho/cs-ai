using System.Collections.Generic;

namespace TicTacToe.Agent.MonteCarlo
{
    internal class Episode
    {
        public List<TicTacToeObservation> Observations { get; private set; }

        public static Episode Generate(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            return new Episode {Observations = CreateEpisode(agent, opponent)};
        }

        private static List<TicTacToeObservation> CreateEpisode(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            var env = new TicTacToeEnvironment(opponent);

            var lastObservation = env.Reset();
            var observations = new List<TicTacToeObservation> { lastObservation };

            while (!lastObservation.IsDone)
            {
                lastObservation = env.Step(agent.GetAction(env, lastObservation));
                observations.Add(lastObservation);
            }

            return observations;
        }
    }
}