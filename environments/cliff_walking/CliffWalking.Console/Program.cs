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

            var values = agent.ImproveEstimates(env);

            RenderValues(values);
        }

        private static void RenderValues(StateActionValues values)
        {
            for (var y = 3; y >= 0; y--)
            {
                var vals = Enumerable.Range(0, 12)
                    .Select(x => RenderPosition(values, new Position(x, y)));
                var line = string.Join(" ", vals);
                System.Console.WriteLine(line);
            }
        }

        private static string RenderPosition(StateActionValues values, Position pos)
        {
            var posValues = values.ActionValues(pos).Select(av => av.Item2).ToList();
            if (posValues.Count == 0)
                return "      ";

            const string format = "{0:0000.0;-000.0}";
            return string.Format(format, posValues.Average());
        }
    }
}
