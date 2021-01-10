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
            var env = new CliffWalkingEnvironment();
            var agent = new QLearningCliffWalker();

            var values = agent.ImproveEstimates(env, out var diagnostics, 500);

            var plotter = new Plotter();
            var plt = plotter.Plt;

            plt.Title("Hello");
            var dataX = Enumerable.Range(0, 500).Select(i => (double) i).ToArray();
            plt.PlotScatter(dataX, diagnostics.RewardSumPerEpisode.ToArray(), label: "stuff");

            plotter.Show();
        }
    }
}
