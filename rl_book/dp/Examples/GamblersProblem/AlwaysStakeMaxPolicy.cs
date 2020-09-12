using System;

namespace dp.Examples.GamblersProblem
{
    class AlwaysStakeMaxPolicy : IGamblersPolicy
    {
        private readonly GamblersWorld _world;

        public AlwaysStakeMaxPolicy(GamblersWorld world)
        {
            _world = world;
        }

        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            var stake = Math.Min(state.DollarsInHand, _world.DollarsToWin - state.DollarsInHand);

            return action.Stake == stake ? 1 : 0;
        }
    }
}
