using System;
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
    /// <summary>
    /// One-step temporal difference learning agent. Off-policy (?). Learns afterstate-value
    /// function, ie. the value of a state gets updated based on the value of the next
    /// state after the agent's move:
    ///
    ///     state ---agent move---> state ---opponent move---> state
    ///       ^                                                  |
    ///       |__________________________________________________|
    ///                     update value estimate
    /// 
    /// This negates the need to model the opponent's behaviour, as it becomes part of
    /// the value function.
    ///
    /// This is the same agent as described in the RL book's intro.
    /// </summary>
    public class Td0Agent : ITicTacToeAgent
    {
        public BoardTile Tile { get; }

        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private const double LearningRate = 0.05;
        private readonly Random _random = new();

        private StateValueTable _values;

        public Td0Agent(BoardTile tile)
        {
            Tile = tile;
            _values = new StateValueTable(Tile);
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
                env.Reset();
                Board? previousAfterstate = null;

                while (!env.CurrentState.IsGameOver)
                {
                    var isExploratoryAction = ShouldDoExploratoryAction();
                    var action = isExploratoryAction ? RandomAction(env) : BestAction(env.CurrentState);
                    var afterstate = env.CurrentState.DoAction(action);
                    env.Step(action);

                    // Note that values are not updated after exploratory moves.
                    // Does this make this off-policy learning? If yes, why is
                    // there no importance sampling?
                    if (previousAfterstate != null && !isExploratoryAction)
                    {
                        var tdError = _values.Value(afterstate) - _values.Value(previousAfterstate);
                        // Note that reward is not included here, as the value table pre-defines
                        // game-over state values. Alternatively, we would need a special terminal
                        // state after game-over, that has zero reward for transitioning to.
                        var updatedValue = _values.Value(previousAfterstate) + LearningRate * tdError;

                        _values.SetValue(previousAfterstate, updatedValue);
                    }
                    previousAfterstate = afterstate;
                }
            }

            Console.WriteLine($"Played {gameCount} games in {stopwatch.ElapsedMilliseconds} ms");
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
