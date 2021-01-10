using System.Collections.Generic;

namespace CliffWalking.Agent
{
    public class TrainingDiagnostics
    {
        public List<double> RewardSumPerEpisode { get; set; } = new();
    }
}