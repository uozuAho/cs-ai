using System;
using vacuum_world.Utils;

namespace vacuum_world
{
    public static class VacuumWorldGenerator
    {
        public static VacuumWorldState CreateWorldWithRandomlyDirtySquares(int worldSize)
        {
            var state = new VacuumWorldState(worldSize);
            var random = new Random();
            
            for (var i = 0; i < state.WorldSize; i++)
            {
                for (var j = 0; j < state.WorldSize; j++)
                {
                    if (random.TrueWithProbability(0.5))
                    {
                        state.MakeSquareDirty(i, j);
                    }
                }
            }

            return state;
        }
    }
}