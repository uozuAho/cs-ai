using System;
using random_walk.Playground.mc;

namespace random_walk.Playground
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            RandomWalkMcValueEstimator.Run();
        }
    }
}
