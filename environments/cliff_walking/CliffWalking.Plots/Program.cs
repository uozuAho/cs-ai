using System;

namespace CliffWalking.Plots
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // LearningAgentsInterimComparison.Run();
            // LearningAgentsInterimVsAsymptotic.Run();
            QLearningVsDynaQ.Run();
        }
    }
}
