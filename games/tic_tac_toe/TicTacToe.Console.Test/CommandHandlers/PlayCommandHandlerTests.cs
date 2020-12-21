using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class PlayCommandHandlerTests
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
            var runner = new PlayCommandHandler(_output, new PlayerRegister());
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
            var runner = new PlayCommandHandler(_output, new PlayerRegister());
            runner.Run("FirstAvailableSlotAgent", "FirstAvailableSlotAgent", numGames);

            // assert
            _output.ReadToEnd();

            const int expectedNumXWins = numGames;
            const int expectedNumOWins = 0;
            _output.ExpectLine(-1, $"After {numGames} games, x wins, o wins: " +
                                   $"{expectedNumXWins}, {expectedNumOWins}");
        }
    }
}