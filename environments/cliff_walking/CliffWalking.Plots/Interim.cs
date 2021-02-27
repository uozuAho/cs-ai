using System.Linq;
using CliffWalking.Agent;
using WpfPlotter;

namespace CliffWalking.Plots
{
    /// <summary>
    /// Plots a comparison of interim performance, like Example 6.6, Cliff Walking
    /// </summary>
    class Interim
    {
        public static void Run()
        {
            const int numEpisodes = 100;
            var env = new CliffWalkingEnvironment();
            var sarsaAgent = new SarsaCliffWalker(0.1, 0.1);
            var qLearningAgent = new QLearningCliffWalker(0.1, 0.1);

            var tdAverageRewards = CollectAverageRewardSums(sarsaAgent, env, numEpisodes);
            var qLearningAverageRewards = CollectAverageRewardSums(qLearningAgent, env, numEpisodes);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Average total reward per episode");
            var dataX = Enumerable.Range(0, numEpisodes).Select(i => (double)i).ToArray();
            plt.PlotScatter(dataX, tdAverageRewards, label: "TD 0 (Sarsa)");
            plt.PlotScatter(dataX, qLearningAverageRewards, label: "Q learning");

            plt.XLabel("Episodes");
            plt.YLabel("Average total reward");
            plt.Legend();

            plotter.Show();
        }

        private static double[] CollectAverageRewardSums(
            ICliffWalkingAgent agent, CliffWalkingEnvironment env, int numEpisodes, int numRuns = 500)
        {
            var rewardSumSums = new double[numEpisodes];

            for (var i = 0; i < numRuns; i++)
            {
                agent.ImproveEstimates(env, out var diags, numEpisodes);
                for (var episode = 0; episode < numEpisodes; episode++)
                {
                    rewardSumSums[episode] += diags.RewardSumPerEpisode[episode];
                }
            }

            return rewardSumSums.Select(s => s / numRuns).ToArray();
        }
    }
}
