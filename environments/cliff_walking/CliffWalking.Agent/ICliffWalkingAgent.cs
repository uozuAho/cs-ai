namespace CliffWalking.Agent
{
    public interface ICliffWalkingAgent
    {
        IStateActionValues ImproveEstimates(
            CliffWalkingEnvironment env, out TrainingDiagnostics diagnostics, int numEpisodes);
    }
}