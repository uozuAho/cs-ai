using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Game;

namespace TicTacToe.Console.Test
{
    internal class PlayerRegisterTests
    {
        [Test]
        public void FindsPlayerByName()
        {
            var register = new PlayerRegister();

            Assert.AreEqual(typeof(FirstAvailableSlotPlayer),
                register.GetPlayerByName("FirstAvailableSlotPlayer", BoardTile.O).GetType());

            Assert.AreEqual(typeof(ConsoleInputTicTacToePlayer),
                register.GetPlayerByName("ConsoleInputTicTacToePlayer", BoardTile.O).GetType());
        }

        [Test]
        public void CreatesPlayerWithCorrectTile()
        {
            var register = new PlayerRegister();

            Assert.AreEqual(BoardTile.X,
                register.GetPlayerByName("FirstAvailableSlotPlayer", BoardTile.X).Tile);

            Assert.AreEqual(BoardTile.O,
                register.GetPlayerByName("FirstAvailableSlotPlayer", BoardTile.O).Tile);
        }
    }
}
