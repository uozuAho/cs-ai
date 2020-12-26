using System.Text.Json;
using NUnit.Framework;

namespace TicTacToe.Game.Test
{
    internal class SerializableBoardTests
    {
        [Test]
        public void Serialize_Deserialize_AreTheSame()
        {
            const BoardTile currentPlayer = BoardTile.X;
            var board = Board.CreateEmptyBoard(currentPlayer);

            var serializableBoard = SerializableBoard.FromBoard(board);

            // act
            var serializedBoard = JsonSerializer.Serialize(serializableBoard);
            var deserializedBoard = JsonSerializer.Deserialize<SerializableBoard>(serializedBoard);

            // assert
            Assert.NotNull(deserializedBoard);
            Assert.AreEqual(serializableBoard, deserializedBoard);
            Assert.AreEqual(board, deserializedBoard.ToBoard());
        }
    }
}
