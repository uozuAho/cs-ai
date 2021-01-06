using System.Text.Json;
using NUnit.Framework;
using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    public class SerializableStateActionTests
    {
        [Test]
        public void Serialize_Deserialize_AreSame()
        {
            var stateActionPair = new SerializableStateAction(
                SerializableBoard.FromBoard(Board.CreateEmptyBoard()),
                SerializableTicTacToeAction.FromAction(new TicTacToeAction { Position = 3, Tile = BoardTile.X }));

            var serialized = JsonSerializer.Serialize(stateActionPair);
            var deserialized = JsonSerializer.Deserialize<SerializableStateAction>(serialized);

            Assert.AreEqual(stateActionPair, deserialized);
        }
    }
}