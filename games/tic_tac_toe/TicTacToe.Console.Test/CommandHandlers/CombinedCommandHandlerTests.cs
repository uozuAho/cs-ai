using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class CombinedCommandHandlerTests
    {
        private TestUserOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            const string trainedAgentName = "mc_vs_firstSlot";

            var trainer = new TrainCommandHandler(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", trainedAgentName);

            var lister = new ListCommandHandler(new PlayerRegister(), _output);
            lister.Run();

            Assert.True(_output.ContainsLine(line => line.Contains(trainedAgentName)));
        }

        [Test]
        public void AfterTrain_TrainedAgentIsPlayable()
        {
            const string agentName = "mc_agent";
            var trainer = new TrainCommandHandler(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", agentName);

            var register = new PlayerRegister();
            register.LoadPolicyFiles();
            var runner = new PlayCommandHandler(_output, register);
            runner.Run(agentName, "FirstAvailableSlotAgent");

            _output.ReadToEnd();

            _output.ExpectLine(-1, "The winner is: X!");
        }
    }
}