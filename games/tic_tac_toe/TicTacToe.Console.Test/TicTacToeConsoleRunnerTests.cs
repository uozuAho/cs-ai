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
            _user.WillEnterLines("b", "b", "1");

            // act
            _ticTacToeRunner.Run();

            // assert
            _output.ExpectLines(
                "Player choices:",
                "  a: ConsoleInputPlayer",
                "  b: FirstAvailableSlotAgent",
                "  c: PTableAgent",
                "  d: ModifiedPTableAgent",
                "Choose player 1 (x)",
                "Choose player 2 (o)",
                "How many games? (more than 5 runs headless)");

            _output.ReadToEnd();

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
            _user.WillEnterLines("b", "b", $"{numGames}");

            // act
            _ticTacToeRunner.Run();

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
            _ticTacToeRunner.Run("train", "mc", "FirstAvailableSlotAgent");

            _output.ExpectLine("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
        }
    }
}