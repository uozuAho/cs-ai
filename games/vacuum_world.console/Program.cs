using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ailib.Algorithms.Search;

namespace vacuum_world.console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var size = int.Parse(args[0]);
            var initialState = VacuumWorldGenerator.CreateWorldWithRandomlyDirtySquares(size);
            var problem = new VacuumWorldSearchProblem(initialState);
            
            // bfs is too inefficient to solve more than ~5x5 worlds. For 5x5 world with all dirty squares, the number
            // of possible states =
            //       2    (square is dirty or clean)
            //    ^ 25    (25 squares)
            //    * 25    (25 possible vacuum locations)
            //    = 838 860 800
            // var solver = new BreadthFirstSearch<VacuumWorldState, VacuumWorldAction>(problem);
            
            // greedy best first is much more efficient, but won't find an optimal solution, ie. number of
            // moves to clean all squares may be more tstatehan the minimum possible
            // var solver = new GreedyBestFirstSearch<VacuumWorldState, VacuumWorldAction>(problem, NumDirtySquares);
            
            // - NumDirtySquares results in searching too many states
            // - 5 * NumDirtySquares finishes quickly, but doesn't look optimal
            var solver = new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, state =>
                state.MinNumberOfMovesToDirtySquare() + 2 * state.NumDirtySquares() - 1);

            Console.WriteLine($"Finding cleaning solution for {size}x{size} world...");
            
            var stats = SolveAndRecordStats(solver);
            
            PrintStats(stats);

            InteractivelyDisplaySolution(initialState, solver.GetSolution());
        }

        private static void PrintStats(SolverStats stats)
        {
            Console.WriteLine($"{nameof(stats.TimeToFinish)}:            {stats.TimeToFinish}");
            Console.WriteLine($"{nameof(stats.FoundSolution)}:           {stats.FoundSolution}");
            Console.WriteLine($"{nameof(stats.SolutionCost)}:            {stats.SolutionCost}");
            Console.WriteLine($"{nameof(stats.NumberOfExploredStates)}:  {stats.NumberOfExploredStates}");
        }
        
        private static SolverStats SolveAndRecordStats(ISearchAlgorithm<VacuumWorldState, VacuumWorldAction> solver)
        {
            var stats = new SolverStats();
            
            var stopwatch = Stopwatch.StartNew();
            solver.Solve();

            stats.TimeToFinish = stopwatch.Elapsed;
            stats.NumberOfExploredStates = solver.NumberOfExploredStates;
            stats.FoundSolution = solver.IsSolved;
            stats.SolutionCost = solver.GetSolution().Count();

            return stats;
        }

        private static void InteractivelyDisplaySolution(
            VacuumWorldState initialState,
            IEnumerable<VacuumWorldAction> solution)
        {
            var renderer = new Renderer();
            var machine = new VacuumWorldStateMachine(initialState);

            foreach (var action in solution)
            {
                renderer.Render(machine.State);
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
                machine.DoAction(action);
            }
            
            renderer.Render(machine.State);
        }

        private class SolverStats
        {
            public TimeSpan TimeToFinish { get; set; }
            public bool FoundSolution { get; set; }
            public int NumberOfExploredStates { get; set; }
            public int SolutionCost { get; set; }
        }
    }
}
