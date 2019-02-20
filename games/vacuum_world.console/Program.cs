using System;
using System.Diagnostics;
using ailib.Algorithms.Search;

namespace vacuum_world.console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var size = int.Parse(args[0]);
            var initialState = new VacuumWorldState(size);
            RandomlyMakeSquaresDirty(initialState);
            
            var problem = new VacuumWorldSearchProblem(initialState);
            var bfs = new BreadthFirstSearch<VacuumWorldState, VacuumWorldAction>(problem);

            var stopwatch = Stopwatch.StartNew();
            bfs.Solve();
            var solveTimeMs = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Ran BFS in {solveTimeMs}ms");

            if (!bfs.IsSolved)
            {
                Console.WriteLine("no solution!");
                return;
            }

            var machine = new VacuumWorldStateMachine(initialState);

            foreach (var action in bfs.GetSolution())
            {
                DrawWorld(machine.State);
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
                machine.DoAction(action);
            }
            
            DrawWorld(machine.State);
            
            Console.WriteLine("all clean!");
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
