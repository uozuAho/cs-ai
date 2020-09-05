namespace dp.GamblersProblem
{
    internal class GamblersWorldRewarder :
        IGamblersWorldRewarder,
        IGenericRewarder<GamblersWorldState, GamblersWorldAction>
    {
        private readonly GamblersWorld _world;

        public GamblersWorldRewarder(GamblersWorld world)
        {
            _world = world;
        }

        public double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action)
        {
            if (_world.IsTerminal(oldState)) return 0.0;
            if (newState.DollarsInHand == _world.DollarsToWin) return 1;
            return 0;
        }
    }
}