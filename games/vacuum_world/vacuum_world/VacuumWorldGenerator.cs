using System;

namespace vacuum_world
{
    public class VacuumWorldGenerator
    {
        public static VacuumWorldState CreateWorldWithRandomlyDirtySquares(int worldSize)
        {
            var state = new VacuumWorldState(worldSize);
            var random = new Random();
            
            for (var i = 0; i < state.WorldSize; i++)
            {
                for (var j = 0; j < state.WorldSize; j++)
                {
                    state.SetSquareIsDirty(i, j, random.NextDouble() < 0.5);
                }
            }

            return state;
        }
    }
}