using System.Collections.Generic;
using System.Linq;

namespace CliffWalking.Console
{
    internal class ConsolePathRenderer
    {
        public static void RenderPath(IEnumerable<Position> path)
        {
            var pathSet = path.ToHashSet();

            for (var y = 3; y >= 0; y--)
            {
                var vals = Enumerable.Range(0, 12)
                    .Select(x => pathSet.Contains(new Position(x, y)) ? "x" : ".");
                var line = string.Join("", vals);
                System.Console.WriteLine(line);
            }
        }
    }
}