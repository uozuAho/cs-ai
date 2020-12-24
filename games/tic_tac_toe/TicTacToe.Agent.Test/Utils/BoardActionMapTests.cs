using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class BoardActionMapTests
    {
        [Test]
        public void LoadSavedMap_GetsSameMap()
        {
            const string filePath = "BoardActionMapTests_LoadSave.test.json";
            var map = new BoardActionMap();

            map.SetAction(Board.CreateEmptyBoard(), new TicTacToeAction {Position = 2, Tile = BoardTile.O});

            // act
            map.SaveToFile(filePath);
            var loadedMap = BoardActionMap.LoadFromFile(filePath);

            // assert
            var originalActions = map.AllActions().OrderBy(a => a.Item1.ToString()).ToList();
            var loadedActions = loadedMap.AllActions().OrderBy(a => a.Item1.ToString()).ToList();

            CollectionAssert.AreEqual(originalActions, loadedActions);
        }

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
            var map = BoardActionMap.FromJsonString(mapJson);
            
            Assert.AreEqual(1, map.NumStates);
            Assert.AreEqual(5, map.ActionFor(Board.CreateFromString(boardString, BoardTile.O)).Position);
        }
    }
}
