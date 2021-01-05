using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class PolicyFileLoaderTests
    {
        [Test]
        public void FromString()
        {
            const string boardString = "x  | o |   ";
            var mapJson = $@"
{{
    ""name"": ""billy"",
    ""description"": ""stuff"",
    ""tile"": ""O"",
    ""actions"": [{{
        ""board"": ""{boardString}"",
        ""value"": 2.22,
        ""action"": 5
    }}]
}}";
            var file = PolicyFileLoader.FromJsonString(mapJson);

            Assert.AreEqual("billy", file.Name);
            Assert.AreEqual("stuff", file.Description);
            Assert.AreEqual(BoardTile.O, file.Tile);
        }
    }
}
