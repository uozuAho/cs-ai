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

            td0Agent.ImproveEstimates(env, out var tdDiags, numEpisodes);
            qLearningAgent.ImproveEstimates(env, out var qLearningDiags, numEpisodes);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Sum of rewards per episode");
            var dataX = Enumerable.Range(0, numEpisodes).Select(i => (double) i).ToArray();
            plt.PlotScatter(dataX, tdDiags.RewardSumPerEpisode.ToArray(), label: "TD 0 (Sarsa)");
            plt.PlotScatter(dataX, qLearningDiags.RewardSumPerEpisode.ToArray(), label: "Q learning");

            plotter.Show();
        }
    }
}
