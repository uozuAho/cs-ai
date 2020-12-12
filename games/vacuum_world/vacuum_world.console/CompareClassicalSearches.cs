using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ailib.Algorithms.Search;
using vacuum_world.ActionHandlers;
using vacuum_world.Actions;

namespace vacuum_world.console
{
    internal static class CompareClassicalSearches
    {
        public static void Run(string[] args)
        {
            var size = int.Parse(args[0]);
            var initialState = VacuumWorldGenerator.CreateWorldWithRandomlyDirtySquares(size);
            var problem = new VacuumWorldSearchProblem(initialState,
                VacuumWorldActionHandler.CreateDeterministicActionHandler());

            var solvers = new List<SearchAlgorithmWithLabel>
            {
                // BFS
                // bfs is too inefficient to solve more than ~5x5 worlds. For 5x5 world with all dirty squares, the number
                // of possible states =
                //       2    (square is dirty or clean)
                //    ^ 25    (25 squares)
                //    * 25    (25 possible vacuum locations)
                //    = 838 860 800
                new SearchAlgorithmWithLabel("BFS",
                    new BreadthFirstSearch<VacuumWorldState, VacuumWorldAction>(problem)
                ),
                
                // Greedy best first
                // greedy best first is much more efficient, but won't find an optimal solution, ie. number of
                // moves to clean all squares may be more than the minimum possible
                new SearchAlgorithmWithLabel("Greedy best first, heuristic: number of dirty squares",
                    new GreedyBestFirstSearch<VacuumWorldState, VacuumWorldAction>(problem, s => s.NumberOfDirtySquares())
                ),
                
                // A*
                // A* is optimal and optimally efficient, given the heuristic is admissible (always
                // underestimates) and consistent (monotonic decreasing).
                
                // h = number of dirty squares
                // - admissible & consistent
                // - too much of an underestimate, results in searching too many states 
                new SearchAlgorithmWithLabel("A*, heuristic: number of dirty squares",
                    new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, s => s.NumberOfDirtySquares())
                ),
                
                // h = 2 * number of dirty squares
                // - closer to the truth - need to move to each square, and clean it (2 moves per square)
                // - admissible & consistent
                // - a closer underestimate, searches fewer than the above h
                new SearchAlgorithmWithLabel("A*, heuristic: 2 x number of dirty squares",
                    new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, s => 2 * s.NumberOfDirtySquares())
                ),
                
                // h = 5 * number of dirty squares
                // - an overestimate to improve solution time
                // - not admissible, is it consistent?
                new SearchAlgorithmWithLabel("A*, heuristic: 5 x number of dirty squares",
                    new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, s => 5 * s.NumberOfDirtySquares())
                ),
                
                // h = 2 * number of dirty squares + number of moves to closest dirty square - 1
                // This seems like a pretty accurate guess
                // - Admissible and consistent (I think)
                // - Calculating number of moves to dirty square is inefficient
                new SearchAlgorithmWithLabel("A*, heuristic: 2 x number of dirty squares + distance to closest dirty square - 1",
                    new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, state => 
                        state.MinNumberOfMovesToDirtySquare() + 2 * state.NumberOfDirtySquares() - 1)
                ),
            };

            var numDirtySquares = initialState.NumberOfDirtySquares();
            Console.WriteLine($"Comparing search algorithms for {size}x{size} world with {numDirtySquares} dirty squares...");

            foreach (var solver in solvers)
            {
                Console.WriteLine($"Running solver: {solver.Label}");
                
                var stats = SolveAndRecordStats(solver.Algorithm);
            
                PrintStats(stats);
                
                Console.WriteLine($"---------");
            }
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
            var machine = new VacuumWorld(initialState,
                VacuumWorldActionHandler.CreateDeterministicActionHandler());

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

        private record SearchAlgorithmWithLabel(
            string Label,
            ISearchAlgorithm<VacuumWorldState, VacuumWorldAction> Algorithm)
        {
        }
    }
}