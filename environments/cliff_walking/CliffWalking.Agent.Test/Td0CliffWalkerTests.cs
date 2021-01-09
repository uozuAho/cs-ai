using NUnit.Framework;

namespace CliffWalking.Agent.Test
{
    internal class Td0CliffWalkerTests
    {
        [Test]
        public void AfterImprovingEstimates_StartingPosition_HasActionValues()
        {
            var env = new CliffWalkingEnvironment();
            var agent = new Td0CliffWalker();

            var values = agent.ImproveEstimates(env);

            Assert.IsNotEmpty(values.ActionValues(new Position(0, 0)));
        }
    }
}
