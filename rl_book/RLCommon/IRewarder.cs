namespace dp
{
    public interface IRewarder<in TState, in TAction>
    {
        double Reward(TState state, TState nextState, TAction action);
    }
}