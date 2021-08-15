using System.Collections.Generic;

namespace CliffWalking.Agent.DataStructures
{
    public class TrainingDiagnostics
    {
        public List<double> RewardSumPerEpisode { get; set; } = new();
    }
}