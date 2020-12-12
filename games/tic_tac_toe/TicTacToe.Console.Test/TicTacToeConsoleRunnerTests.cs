using NUnit.Framework;

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
            _user.WillEnterLines("1");

            // act
            _ticTacToeRunner.Run("play", "FirstAvailableSlotAgent", "FirstAvailableSlotAgent");

            // assert
            _output.ExpectLine("How many games? (more than 5 runs headless)");
            _output.ExpectLine("");
            _output.ExpectLine("xox");
            _output.ExpectLine("oxo");
            _output.ExpectLine("x..");
            _output.ExpectLine("");
            _output.ExpectLine("The winner is: X!");
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

        // todo: invalid player/opponent prints help mesg

        [Test]
        public void Train_TrainsAnAgent()
        {
            _ticTacToeRunner.Run("train", "mc", "FirstAvailableSlotAgent");

            _output.ExpectLine("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
        }
    }
}