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
            for (var i = 0; i < state.WorldSize; i++)
            {
                for (var j = 0; j < state.WorldSize; j++)
                {
                    if (state.VacuumPos.Equals(new Point2D(i, j)))
                    {
                        Console.Write("V");
                    }
                    else
                    {
                        Console.Write(state.GetSquare(i, j).IsDirty ? "X" : ".");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
