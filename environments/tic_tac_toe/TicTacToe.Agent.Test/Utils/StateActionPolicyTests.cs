using NUnit.Framework;
using TicTacToe.Agent.Storage;
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
            var stateValues = new StateValueTable(BoardTile.X);

            PolicyFileIo.Save(stateValues, "name", "description", filePath);
            var loadedStateValues = PolicyFileIo.LoadStateValueTable(filePath);

            Assert.IsNotNull(loadedStateValues);
        }
    }
}