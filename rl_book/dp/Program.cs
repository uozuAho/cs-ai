using dp.Examples.GamblersProblem;
using dp.Examples.GridWorld;

namespace dp
{
    class Program
    {
        static void Main(string[] args)
        {
            GridWorldPolicyEvaluation.Run();
            GridWorldPolicyIteration.Run();
            GamblersProblemExample.Run();
        }
    }
}
