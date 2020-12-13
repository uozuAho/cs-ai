using NUnit.Framework;
using TicTacToe.Agent;
using TicTacToe.Game;

namespace TicTacToe.Console.Test
{
    internal class PlayerRegisterTests
    {
        [Test]
        public void FindsPlayerByName()
        {
            var register = new PlayerRegister();

            Assert.AreEqual(typeof(FirstAvailableSlotAgent),
                register.GetPlayerByName("FirstAvailableSlotAgent", BoardTile.O).GetType());

            Assert.AreEqual(typeof(ConsoleInputTicTacToePlayer),
                register.GetPlayerByName("ConsoleInputTicTacToePlayer", BoardTile.O).GetType());
        }

        [Test]
        public void CreatesPlayerWithCorrectTile()
        {
            var register = new PlayerRegister();

            Assert.AreEqual(BoardTile.X,
                register.GetPlayerByName("FirstAvailableSlotAgent", BoardTile.X).Tile);

            Assert.AreEqual(BoardTile.O,
                register.GetPlayerByName("FirstAvailableSlotAgent", BoardTile.O).Tile);
        }
    }
}
