using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class PolicyFileTests
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
            var (name, description, boardTile, policyFileActions) = PolicyFile.FromJsonString(mapJson);

            Assert.AreEqual("billy", name);
            Assert.AreEqual("stuff", description);
            Assert.AreEqual(BoardTile.O, boardTile);
            Assert.AreEqual(1, policyFileActions.Length);
        }

        [Test]
        public void LoadSavedMap_GetsSameMap()
        {
            const string filePath = "BoardActionMapTests_LoadSave.test.json";
            var policy = new PolicyFile("abc", "def", BoardTile.X, new[]
            {
                new PolicyFileAction(Board.CreateEmptyBoard().ToString(), 1.23, 4)
            });

            // act
            policy.Save(filePath);
            var loadedPolicy = PolicyFile.Load(filePath);

            // assert
            var originalActions = policy.Actions.OrderBy(a => a.Board).ToList();
            var loadedActions = loadedPolicy.Actions.OrderBy(a => a.Board).ToList();

            CollectionAssert.AreEqual(originalActions, loadedActions);
        }
    }
}
