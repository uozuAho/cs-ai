using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    internal class ReturnsTests
    {
        [Test]
        public void AverageReturnFrom_ReturnsAverage()
        {
            var returns = new Returns();

            var emptyBoard = Board.CreateEmptyBoard();
            var action = new TicTacToeAction();

            returns.Add(emptyBoard, action, 2);
            returns.Add(emptyBoard, action, 4);

            Assert.AreEqual(3, returns.AverageReturnFrom(emptyBoard, action));
        }
    }
}
