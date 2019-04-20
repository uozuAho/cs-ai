using System;
using System.Collections.Generic;

namespace vacuum_world
{
    public static class VacuumWorldStateExtensions
    {
        public static int NumberOfDirtySquares(this VacuumWorldState state)
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
        
        /// <summary>
        /// Warning! this is inefficient
        /// </summary>
        public static int MinNumberOfMovesToDirtySquare(this VacuumWorldState state)
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
        
        public static void SetAllSquaresDirty(this VacuumWorldState state)
        {
            for (var y = 0; y < state.WorldSize; y++)
            {
                for (var x = 0; x < state.WorldSize; x++)
                {
                    state.MakeSquareDirty(x, y);
                }
            }
        }
        
        public static IEnumerable<Point2D> AdjacentSquares(this VacuumWorldState state, Point2D pos)
        {
            if (pos.X > 0)                   yield return new Point2D(pos.X - 1, pos.Y);
            if (pos.X < state.WorldSize - 1) yield return new Point2D(pos.X + 1, pos.Y);
            if (pos.Y > 0)                   yield return new Point2D(pos.X, pos.Y - 1);
            if (pos.Y < state.WorldSize - 1) yield return new Point2D(pos.X, pos.Y + 1);
        }
    }
}