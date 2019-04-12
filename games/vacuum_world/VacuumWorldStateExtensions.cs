using System;

namespace vacuum_world
{
    public static class VacuumWorldStateExtensions
    {
        public static int NumDirtySquares(this VacuumWorldState state)
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
    }
}