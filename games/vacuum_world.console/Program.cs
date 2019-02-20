using System;
using ailib.Algorithms.Search;

namespace vacuum_world.console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
//            var size = int.Parse(args[0]);
            var size = 3;
            var state = new VacuumWorldState(size);
            RandomlyMakeSquaresDirty(state);
            
            DrawWorld(state);

            var problem = new VacuumWorldSearchProblem(state);
            var bfs = new BreadthFirstSearch<VacuumWorldState, VacuumWorldAction>(problem);

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
                bfs.Step();
                DrawWorld(bfs.CurrentState);
                if (bfs.IsSolved)
                {
                    Console.WriteLine("done!");
                    break;
                }

                if (bfs.IsFinished)
                {
                    Console.WriteLine("no solution!");
                    break;
                }
            }
        }

        private static void RandomlyMakeSquaresDirty(VacuumWorldState state)
        {
            var random = new Random();
            
            for (var i = 0; i < state.WorldSize; i++)
            {
                for (var j = 0; j < state.WorldSize; j++)
                {
                    state.GetSquare(i, j).IsDirty = random.NextDouble() < 0.5;
                }
            }
        }

        private static void DrawWorld(VacuumWorldState state)
        {
            Console.WriteLine(state);
        }
    }
}
