using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class PolicyFileTests
    {
        [Test]
        public void LoadSavedMap_GetsSameMap()
        {
            const string filePath = "BoardActionMapTests_LoadSave.test.json";
            var policy = new PolicyFile("abc", "def", BoardTile.X, new[]
            {
                new PolicyFileAction(Board.CreateEmptyBoard().ToString(), 1.23, 4)
            });

            // act
            PolicyFileLoader.Save(policy, filePath);
            var loadedPolicy = (PolicyFile) PolicyFileLoader.FromFile(filePath);
            Assert.IsNotNull(loadedPolicy);

            // assert
            var originalActions = policy.Actions.OrderBy(a => a.Board).ToList();
            var loadedActions = loadedPolicy.Actions.OrderBy(a => a.Board).ToList();

            CollectionAssert.AreEqual(originalActions, loadedActions);
        }
    }
}