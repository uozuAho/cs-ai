using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ailib.Utils;

namespace CliffWalking.Agent
{
    public class Td0CliffWalker
    {
        // e-greedy constant: probability of choosing a random action instead
        // of the greedy action
        // possible improvement: reduce over time during training
        private const double ChanceOfRandomAction = 0.05;
        private const double LearningRate = 0.05;
        private const double DefaultActionValue = 0.0;
        private readonly Random _random = new();

        private readonly Dictionary<Position, Dictionary<CliffWalkingAction, double>> _stateActionValues = new();

        public void ImproveEstimates(
            CliffWalkingEnvironment environment,
            Dictionary<Position, Dictionary<CliffWalkingAction, double>> actionValues)
        {
            var iterationCount = 0;

            var stopwatch = Stopwatch.StartNew();

            for (; iterationCount < 123; iterationCount++)
            {
                environment.Reset();

                // while (!env.CurrentState.IsGameOver)
                // {
                //     var isExploratoryAction = ShouldDoExploratoryAction();
                //     var action = isExploratoryAction ? RandomAction(env) : BestAction(env.CurrentState);
                //     var afterstate = env.CurrentState.DoAction(action);
                //     env.Step(action);
                //
                //     // Note that values are not updated after exploratory moves.
                //     // Does this make this off-policy learning? If yes, why is
                //     // there no importance sampling?
                //     if (previousAfterstate != null && !isExploratoryAction)
                //     {
                //         var tdError = _values.Value(afterstate) - _values.Value(previousAfterstate);
                //         // Note that reward is not included here, as the value table pre-defines
                //         // game-over state values. Alternatively, we would need a special terminal
                //         // state after game-over, that has zero reward for transitioning to.
                //         var updatedValue = _values.Value(previousAfterstate) + LearningRate * tdError;
                //
                //         _values.SetValue(previousAfterstate, updatedValue);
                //     }
                //     previousAfterstate = afterstate;
                // }
            }

            Console.WriteLine($"Ran {iterationCount} iterations in {stopwatch.ElapsedMilliseconds} ms");
        }

        private CliffWalkingAction BestAction(CliffWalkingEnvironment env, Position currentPosition)
        {
            return env
                .ActionSpace()
                .Select(a => new
                {
                    action = a,
                    value = Value(currentPosition, a)
                })
                .OrderByDescending(av => av.value)
                .First().action;
        }

        private double Value(Position position, CliffWalkingAction action)
        {
            if (_stateActionValues.TryGetValue(position, out var actionValues))
            {
                if (actionValues.TryGetValue(action, out var value))
                {
                    return value;
                }
            }

            return DefaultActionValue;
        }

        private CliffWalkingAction RandomAction(CliffWalkingEnvironment env)
        {
            return _random.Choice(env.ActionSpace());
        }

        private bool ShouldDoExploratoryAction()
        {
            return _random.NextDouble() < ChanceOfRandomAction;
        }
    }
}
