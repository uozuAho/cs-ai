using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class StateActionPolicyTests
    {
        [Test]
        public void LoadSavedMap_IsNotNull()
        {
            const string filePath = "BoardActionMapTests_LoadSave.test.json";
            var policy = new StateActionPolicy("abc", "def", BoardTile.X);
            policy.AddStateAction(Board.CreateEmptyBoard(), 1, 1.23);

            PolicyFileLoader.Save(policy, filePath);
            var loadedPolicy = (StateActionPolicy) PolicyFileLoader.FromFile(filePath);

            Assert.IsNotNull(loadedPolicy);
        }
    }
}