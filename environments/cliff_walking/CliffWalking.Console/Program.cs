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
            var td0Agent = new Td0CliffWalker();

            var values = td0Agent.ImproveEstimates(env, 10000);

            System.Console.WriteLine("td 0 avg Values:");
            StateActionValuesConsoleRenderer.RenderAverageValues(values);
            System.Console.WriteLine("td 0 highest Values:");
            StateActionValuesConsoleRenderer.RenderHighestValues(values);
            System.Console.WriteLine("");
            System.Console.WriteLine("td 0 Greedy path:");
            ConsolePathRenderer.RenderPath(GreedyPath(env, values));

            var qAgent = new QLearningCliffWalker();
            var qValues = qAgent.ImproveEstimates(env, 10000);

            System.Console.WriteLine("");
            System.Console.WriteLine("q learning avg Values:");
            StateActionValuesConsoleRenderer.RenderAverageValues(qValues);
            System.Console.WriteLine("q learning highest Values:");
            StateActionValuesConsoleRenderer.RenderHighestValues(qValues);
            System.Console.WriteLine("");
            System.Console.WriteLine("q learning Greedy path:");
            ConsolePathRenderer.RenderPath(GreedyPath(env, qValues));
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
