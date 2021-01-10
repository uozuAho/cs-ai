using System;
using System.Linq;
using CliffWalking.Agent;
using WpfPlotter;

namespace CliffWalking.Plots
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            const int numEpisodes = 500;
            var env = new CliffWalkingEnvironment();
            var td0Agent = new Td0CliffWalker();
            var qLearningAgent = new QLearningCliffWalker();

            var tdAverageRewards = CollectAverageRewardSums(td0Agent, env, numEpisodes);
            var qLearningAverageRewards = CollectAverageRewardSums(qLearningAgent, env, numEpisodes);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Sum of rewards per episode");
            var dataX = Enumerable.Range(0, numEpisodes).Select(i => (double) i).ToArray();
            plt.PlotScatter(dataX, tdAverageRewards, label: "TD 0 (Sarsa)");
            plt.PlotScatter(dataX, qLearningAverageRewards, label: "Q learning");

            plt.Legend();

            plotter.Show();
        }

        private static double[] CollectAverageRewardSums(
            ICliffWalkingAgent agent, CliffWalkingEnvironment env, int numEpisodes, int numRuns=100)
        {
            var rewardSumSums = new double[numEpisodes];

            for (var i = 0; i < numRuns; i++)
            {
                agent.ImproveEstimates(env, out var diags, numEpisodes);
                for (var j = 0; j < numEpisodes; j++)
                {
                    rewardSumSums[j] += diags.RewardSumPerEpisode[j];
                }
            }

            return rewardSumSums.Select(s => s / numRuns).ToArray();
        }
    }
}
