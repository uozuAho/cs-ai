using NUnit.Framework;
using TicTacToe.Console.Test.Utils;
using TicTacToe.Game;

namespace TicTacToe.Console.Test
{
    public class TicTacToeConsoleRunnerTests
    {
        private TestUserOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayOneCompleteGame()
        {
            // act
            var runner = new InteractiveTicTacToeConsoleRunner(_output, new PlayerRegister());
            runner.Run("FirstAvailableSlotAgent", "FirstAvailableSlotAgent");

            _output.ReadToEnd();

            // assert
            _output.ExpectLine(-6, "");
            _output.ExpectLine(-5, "xox");
            _output.ExpectLine(-4, "oxo");
            _output.ExpectLine(-3, "x..");
            _output.ExpectLine(-2, "");
            _output.ExpectLine(-1, "The winner is: X!");
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayMoreThan5GamesHeadless()
        {
            const int numGames = 6;

            // act
            var runner = new InteractiveTicTacToeConsoleRunner(_output, new PlayerRegister());
            runner.Run("FirstAvailableSlotAgent", "FirstAvailableSlotAgent", numGames);

            // assert
            _output.ReadToEnd();

            const int expectedNumXWins = numGames;
            const int expectedNumOWins = 0;
            _output.ExpectLine(-1, $"After {numGames} games, x wins, o wins: " +
                                   $"{expectedNumXWins}, {expectedNumOWins}");
        }

        [Test]
        public void Train_TrainsAnAgent()
        {
            const string agentName = "mc_agent";
            var trainer = new ConsoleAgentTrainer(_output, new PlayerRegister());

            trainer.Run("FirstAvailableSlotAgent", agentName);

            _output.ExpectLine($"Trained mc agent '{agentName}' against 'FirstAvailableSlotAgent'");
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            const string trainedAgentName = "mc_vs_firstSlot";

            var trainer = new ConsoleAgentTrainer(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", trainedAgentName);

            var register = new PlayerRegister();
            register.LoadPolicyFiles();

            var player = register.GetPlayerByName(trainedAgentName, BoardTile.O);
            Assert.NotNull(player);
        }

        [Test]
        public void AfterTrain_TrainedAgentIsPlayable()
        {
            const string agentName = "mc_agent";
            var trainer = new ConsoleAgentTrainer(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", agentName);

            var register = new PlayerRegister();
            register.LoadPolicyFiles();
            var runner = new InteractiveTicTacToeConsoleRunner(_output, register);
            runner.Run(agentName, "FirstAvailableSlotAgent");

            _output.ReadToEnd();

            _output.ExpectLine(-1, "The winner is: X!");
        }
    }
}