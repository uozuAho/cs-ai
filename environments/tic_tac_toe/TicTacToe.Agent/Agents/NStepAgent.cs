using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ailib.Utils;
using MoreLinq;
using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class NStepAgent
    {
        public BoardTile Tile { get; }

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private const double LearningRate = 0.05;
        private readonly Random _random = new();

        private readonly int _numSteps;
        private StateValueTable _values;

        public NStepAgent(BoardTile tile, int numSteps)
        {
            Tile = tile;
            _values = new StateValueTable(Tile);
            _numSteps = numSteps;
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            return ShouldDoExploratoryAction()
                ? RandomAction(environment)
                : BestAction(environment.CurrentState);
        }

        public StateValueTable GetCurrentStateValues()
        {
            return _values;
        }

        public ITicTacToePolicy GetCurrentValues(string name, string description)
        {
            var policy = new SerializableStateValueTable(name, description, Tile);
            policy.SetStateValues(_values);
            return policy;
        }

        public void SaveTrainedValues(string agentName, string path)
        {
            PolicyFileIo.Save(_values, agentName, "", path);
        }

        public void Train(ITicTacToePlayer opponent, int? numGamesLimit = null)
        {
            var gameCount = 0;
            var maxGames = numGamesLimit ?? 10000;
            var env = new TicTacToeEnvironment(opponent);
            _values = new StateValueTable(Tile);

            var stopwatch = Stopwatch.StartNew();

            for (; gameCount < maxGames; gameCount++)
            {
                var currentState = env.Reset();
                var gameLength = int.MaxValue;
                var tau = 0;
                var states = new List<Board> {currentState};

                for (var t = 0; tau < gameLength - 1; t++)
                {
                    tau = t - _numSteps + 1;

                    if (t < gameLength)
                    {
                        var step = env.Step(GetAction(env));
                        states.Add(step.Board);
                        if (step.Board.IsGameOver)
                            gameLength = t + 1;
                    }

                    if (tau >= 0)
                    {
                        // note that values are updated on exploratory moves,
                        // which differs from the TD0 agent
                        ImproveValueEstimate(tau, gameLength, states);
                    }
                }
            }

            Console.WriteLine($"Played {gameCount} games in {stopwatch.ElapsedMilliseconds} ms");
        }

        private void ImproveValueEstimate(int tau, int gameLength, List<Board> states)
        {
            var stepN = Math.Min(tau + _numSteps, gameLength);
            var tauState = states[tau];
            var nState = states[stepN];
            // Note that reward is not included here, as the value table pre-defines
            // game-over state values. Alternatively, we would need a special terminal
            // state after game-over, that has zero reward for transitioning to.
            //
            // if rewards were to be used:
            // var rewardsSum = rewards.Skip(tau + 1).Take(stepsAhead).Sum();
            var tdError = _values.Value(nState) - _values.Value(tauState);
            var updatedValue = _values.Value(tauState) + LearningRate * tdError;

            _values.SetValue(tauState, updatedValue);
        }

        private TicTacToeAction BestAction(Board board)
        {
            return board
                .AvailableActions()
                .Select(a => new
                {
                    action = a,
                    board = board.DoAction(a)
                })
                .MaxBy(b => _values.Value(b.board))
                .First().action;
        }

        private TicTacToeAction RandomAction(TicTacToeEnvironment env)
        {
            return _random.Choice(env.ActionSpace());
        }

        private bool ShouldDoExploratoryAction()
        {
            return _random.NextDouble() < ChanceOfRandomAction;
        }
    }
}
