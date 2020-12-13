using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Agent.Environment;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class Episode
    {
        public List<EpisodeStep> Steps { get; }
        public int Length => Steps.Count;

        private readonly Dictionary<(Board, TicTacToeAction?), int> _stateActionTimes;

        public Episode(IEnumerable<EpisodeStep> steps)
        {
            Steps = steps.ToList();
            _stateActionTimes = CreateStateActionTimeLookup(Steps);
        }

        public static Episode Generate(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            return new(GenerateEpisode(agent, opponent));
        }

        public int TimeOfFirstVisit(Board state, TicTacToeAction action)
        {
            return _stateActionTimes[(state, action)];
        }

        private Dictionary<(Board, TicTacToeAction?), int> CreateStateActionTimeLookup(ICollection steps)
        {
            var lookup = new Dictionary<(Board, TicTacToeAction?), int>();

            for (var i = 0; i < steps.Count; i++)
            {
                var state = Steps[i].State;
                var action = Steps[i].Action;
                lookup[(state, action)] = i;
            }

            return lookup;
        }

        private static IEnumerable<EpisodeStep> GenerateEpisode(ITicTacToeAgent agent, ITicTacToeAgent opponent)
        {
            var env = new TicTacToeEnvironment(opponent);

            var board = env.Reset();
            var done = false;
            var lastReward = 0.0;

            while (!done)
            {
                var action = agent.GetAction(env);

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