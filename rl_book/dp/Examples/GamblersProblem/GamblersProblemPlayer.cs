using System;
using System.Linq;
using MoreLinq;

namespace dp.Examples.GamblersProblem
{
    class GamblersProblemPlayer
    {
        private readonly GamblersWorld _world;
        private readonly IGamblersPolicy _policy;

        public GamblersProblemPlayer(GamblersWorld world, IGamblersPolicy policy)
        {
            _world = world;
            _policy = policy;
        }

        public void Play()
        {
            const int numGamesPerStartingState = 100;
            var numStates = _world.AllStates().Count();
            var numWins = new int[numStates];

            for (var startingDollars = 1; startingDollars < numStates; startingDollars++)
            {
                var startingState = new GamblersWorldState(startingDollars);

                for (var i = 0; i < numGamesPerStartingState; i++)
                {
                    var outcome = PlaySingleGame(startingState);

                    if (_world.IsWin(outcome))
                    {
                        numWins[startingDollars]++;
                    }
                }
            }

            Console.WriteLine("num wins:");
            for (var startingDollars = 0; startingDollars < numStates; startingDollars++)
            {
                Console.WriteLine($"${startingDollars}: {numWins[startingDollars]} wins");
            }
        }

        private GamblersWorldState PlaySingleGame(GamblersWorldState initialState)
        {
            var state = initialState;

            while (!_world.IsTerminal(state))
            {
                var action = MaxAction(state);

                state = _world.NextState(state, action);
            }

            return state;
        }

        private GamblersWorldAction MaxAction(GamblersWorldState state)
        {
            return _world
                .AvailableActions(state)
                .MaxBy(action => _policy.PAction(state, action))
                .First();
        }
    }
}
