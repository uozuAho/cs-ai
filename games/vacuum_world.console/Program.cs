using System;
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
            
            Console.WriteLine($"Finding cleaning solution for {size}x{size} world...");
            
            var initialState = new VacuumWorldState(size);
            RandomlyMakeSquaresDirty(initialState);
            
            var problem = new VacuumWorldSearchProblem(initialState);
            
            // bfs is too inefficient to solve more than ~5x5 worlds. For 5x5 world with all dirty squares, the number
            // of possible states =
            //       2    (square is dirty or clean)
            //    ^ 25    (25 squares)
            //    * 25    (25 possible vacuum locations)
            //    = 838 860 800
            // var solver = new BreadthFirstSearch<VacuumWorldState, VacuumWorldAction>(problem);
            
            // greedy best first is much more efficient, but won't find an optimal solution, ie. number of
            // moves to clean all squares may be more than the minimum possible
//            var solver = new GreedyBestFirstSearch<VacuumWorldState, VacuumWorldAction>(problem, NumDirtySquares);
            
            // todo: is there an optimal heuristic for vacuum world a*?
            // - NumDirtySquares results in searching too many states
            // - 5 * NumDirtySquares finishes quickly, but doesn't look optimal
            var solver = new AStarSearch<VacuumWorldState, VacuumWorldAction>(problem, state =>
                MinNumberOfMovesToDirtySquare(state) + 2 * NumDirtySquares(state) - 1);

            var stopwatch = Stopwatch.StartNew();
            solver.Solve();
            var solveTimeMs = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Solver completed in {solveTimeMs}ms, explored {solver.NumberOfExploredStates} states");

            if (!solver.IsSolved)
            {
                Console.WriteLine("no solution!");
                return;
            }

            var solution = solver.GetSolution().ToList();
            
            Console.WriteLine($"Reached goal in {solution.Count} actions");

            var machine = new VacuumWorldStateMachine(initialState);

            foreach (var action in solution)
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
                    state.SetSquareIsDirty(i, j, random.NextDouble() < 0.5);
                }
            }
        }

        private static void DrawWorld(VacuumWorldState state)
        {
            Console.WriteLine(state);
        }

        private static int NumDirtySquares(VacuumWorldState state)
        {
            var num = 0;
            
            for (var y = 0; y < state.WorldSize; y++)
            {
                for (var x = 0; x < state.WorldSize; x++)
                {
                    if (state.SquareIsDirty(x, y)) num++;
                }
            }

            return num;
        }

        private static int MinNumberOfMovesToDirtySquare(VacuumWorldState state)
        {
            var closestNumMoves = int.MaxValue;

            int NumMoves(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            
            // perf: could check in order of distance from vacuum, and stop when found
            for (var y = 0; y < state.WorldSize; y++)
            {
                for (var x = 0; x < state.WorldSize; x++)
                {
                    if (!state.SquareIsDirty(x, y)) continue;
                    
                    var numMoves = NumMoves(x, y, state.VacuumPos.X, state.VacuumPos.Y);
                        
                    if (numMoves < closestNumMoves)
                    {
                        closestNumMoves = numMoves;
                    }
                }
            }

            return closestNumMoves;
        }
    }
}
