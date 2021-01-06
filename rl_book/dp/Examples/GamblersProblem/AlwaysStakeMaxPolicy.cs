using System;
using RLCommon;

namespace dp.Examples.GamblersProblem
{
    class AlwaysStakeMaxPolicy :
        IGamblersPolicy,
        IPolicy<GamblersWorldState, GamblersWorldAction>,
        IDeterministicPolicy<GamblersWorldState, GamblersWorldAction>
    {
        private readonly GamblersWorld _world;

        public AlwaysStakeMaxPolicy(GamblersWorld world)
        {
            _world = world;
        }

        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            var stake = MaxStake(state);

            return action.Stake == stake ? 1 : 0;
        }

        public GamblersWorldAction Action(GamblersWorldState state)
        {
            return new GamblersWorldAction(MaxStake(state));
        }

        private int MaxStake(GamblersWorldState state)
        {
            return Math.Min(state.DollarsInHand, _world.DollarsToWin - state.DollarsInHand);
        }
    }
}
