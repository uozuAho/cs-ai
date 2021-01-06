using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RLCommon;

namespace dp.Examples.GamblersProblem
{
    public class GamblersWorld : IProblem<GamblersWorldState, GamblersWorldAction>
    {
        public readonly int DollarsToWin;

        private readonly double _probabilityOfHeads;
        private readonly Random _random;
        private readonly List<GamblersWorldState> _allStates;

        public GamblersWorld(double probabilityOfHeads, int dollarsToWin)
        {
            _probabilityOfHeads = probabilityOfHeads;
            DollarsToWin = dollarsToWin;
            _random = new Random();

            _allStates = Enumerable
                .Range(0, DollarsToWin + 1)
                .Select(i => new GamblersWorldState(i))
                .ToList();
        }

        public bool IsWin(in GamblersWorldState state) => state.DollarsInHand == DollarsToWin;

        public bool IsTerminal(GamblersWorldState state) =>
            state.DollarsInHand == 0 || state.DollarsInHand == DollarsToWin;

        public IEnumerable<GamblersWorldState> AllStates()
        {
            return _allStates;
        }

        public IEnumerable<GamblersWorldAction> AvailableActions(GamblersWorldState state)
        {
            if (IsTerminal(state)) return Enumerable.Empty<GamblersWorldAction>();

            var maxStake = Math.Min(state.DollarsInHand, DollarsToWin - state.DollarsInHand);

            return Enumerable.Range(1, maxStake).Select(i => new GamblersWorldAction(i));
        }

        public IEnumerable<(GamblersWorldState, double)>
            PossibleStates(GamblersWorldState state, GamblersWorldAction action)
        {
            Debug.Assert(action.Stake <= state.DollarsInHand);

            yield return (
                new GamblersWorldState(state.DollarsInHand + action.Stake),
                _probabilityOfHeads
            );

            yield return (
                new GamblersWorldState(state.DollarsInHand - action.Stake),
                1.0 - _probabilityOfHeads
            );
        }

        public GamblersWorldState NextState(GamblersWorldState state, GamblersWorldAction action)
        {
            Debug.Assert(action.Stake <= state.DollarsInHand);

            return _random.NextDouble() < _probabilityOfHeads
                ? new GamblersWorldState(state.DollarsInHand + action.Stake)
                : new GamblersWorldState(state.DollarsInHand - action.Stake);
        }
    }
}