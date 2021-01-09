using System.Collections.Generic;
using System.Linq;
using CliffWalking.Agent;

namespace CliffWalking.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new CliffWalkingEnvironment();
            var agent = new Td0CliffWalker();

            var values = agent.ImproveEstimates(env, 10000);

            System.Console.WriteLine("Values:");
            StateActionValuesConsoleRenderer.RenderValues(values);
            System.Console.WriteLine("");
            System.Console.WriteLine("Greedy path:");
            ConsolePathRenderer.RenderPath(GreedyPath(env, values));

            values = agent.ImproveEstimates(env, 50000);

            System.Console.WriteLine("Values:");
            StateActionValuesConsoleRenderer.RenderValues(values);
            System.Console.WriteLine("");
            System.Console.WriteLine("Greedy path:");
            ConsolePathRenderer.RenderPath(GreedyPath(env, values));

            values = agent.ImproveEstimates(env, 500000);

            System.Console.WriteLine("Values:");
            StateActionValuesConsoleRenderer.RenderValues(values);
            System.Console.WriteLine("");
            System.Console.WriteLine("Greedy path:");
            ConsolePathRenderer.RenderPath(GreedyPath(env, values));
        }

        private static IEnumerable<Position> GreedyPath(
            CliffWalkingEnvironment env, StateActionValues values)
        {
            var currentPosition = env.Reset();
            var isDone = false;

            while (!isDone)
            {
                yield return currentPosition;

                var bestAction = values
                    .ActionValues(currentPosition)
                    .OrderBy(av => av.Item2)
                    .Last().Item1;

                var (observation, _, done) = env.Step(bestAction);
                currentPosition = observation;
                isDone = done;
            }

            yield return currentPosition;
        }
    }
}
