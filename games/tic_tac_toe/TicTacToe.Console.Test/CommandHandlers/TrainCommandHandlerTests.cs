using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class TrainCommandHandlerTests
    {
        private TestUserOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
        }

        [Test]
        public void Train_TrainsAnAgent()
        {
            const string agentName = "mc";
            var trainer = new TrainCommandHandler(
                _output, new PlayerRegister(), new LearningAgentRegister());

            trainer.Run(agentName, "FirstAvailableSlotPlayer", 1);

            _output.ExpectLine($"Trained mc agent '{agentName}' against 'FirstAvailableSlotPlayer'");
        }
    }
}