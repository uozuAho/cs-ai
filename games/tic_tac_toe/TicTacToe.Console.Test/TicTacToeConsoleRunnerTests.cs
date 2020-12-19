using System.Linq;
using NUnit.Framework;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test
{
    public class TicTacToeConsoleRunnerTests
    {
        private StubUserInput _user;
        private TestUserOutput _output;
        private TicTacToeConsoleRunner _ticTacToeRunner;

        [SetUp]
        public void Setup()
        {
            _user = new StubUserInput();
            _output = new TestUserOutput();
            _ticTacToeRunner = new TicTacToeConsoleRunner(_user, _output, new PlayerRegister());
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayOneCompleteGame()
        {
            // act
            _ticTacToeRunner.Run("play", "FirstAvailableSlotAgent", "FirstAvailableSlotAgent");

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
            _ticTacToeRunner.Run("play", "FirstAvailableSlotAgent", "FirstAvailableSlotAgent", numGames.ToString());

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
            _ticTacToeRunner.Run("train", "mc", "FirstAvailableSlotAgent", agentName);

            _output.ExpectLine($"Trained mc agent '{agentName}' against 'FirstAvailableSlotAgent'");
        }

        [Test]
        public void AfterTrain_NewAgentIsInList()
        {
            _ticTacToeRunner.Run("train", "mc", "FirstAvailableSlotAgent", "mc_vs_firstSlot");
            _ticTacToeRunner.Run("list");

            const string expectedAgent = "mc_vs_firstSlot";
            Assert.True(_output.ContainsLine(line => line.Contains(expectedAgent)),
                $"No line containing '{expectedAgent}' in lines:\n" + string.Join("\n", _output.Lines));
        }

        [Test]
        public void AfterTrain_TrainedAgentIsPlayable()
        {
            _ticTacToeRunner.Run("train", "mc", "FirstAvailableSlotAgent", "mc_agent");
            _ticTacToeRunner.Run("play", "mc_agent", "FirstAvailableSlotAgent");

            _output.ReadToEnd();

            _output.ExpectLine(-1, "The winner is: X!");
        }
    }
}