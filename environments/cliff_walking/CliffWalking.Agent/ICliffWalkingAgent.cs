namespace CliffWalking.Agent
{
    public interface ICliffWalkingAgent
    {
        StateActionValues ImproveEstimates(
            CliffWalkingEnvironment env, out TrainingDiagnostics diagnostics, int numEpisodes);
    }
}