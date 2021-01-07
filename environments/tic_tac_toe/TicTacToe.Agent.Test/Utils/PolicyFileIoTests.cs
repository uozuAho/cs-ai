using System.IO;
using NUnit.Framework;
using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class PolicyFileIoTests
    {
        [Test]
        public void SaveAndLoad()
        {
            const string name = "name";
            const string description = "description";
            const string filename = "PolicyFileIoTests.SaveAndLoad.json";

            var stateValues = new StateValueTable(BoardTile.X);
            PolicyFileIo.Save(stateValues, name, description, filename);
            var policy = PolicyFileIo.FromFile("PolicyFileIoTests.SaveAndLoad.json");

            Assert.AreEqual(name, policy.Name);
            Assert.AreEqual(description, policy.Description);
        }

        // invalid files can occur after file format changes due to leftovers from old tests
        [Test]
        public void TryFromFile_ReturnsFalse_WhenFileIsInvalid()
        {
            const string path = "TryFromFile_ReturnsFalse_WhenFileIsInvalid.invalidFile.json";
            File.WriteAllText(path, "{}");

            var result = PolicyFileIo.TryFromFile(path, out var policy);

            Assert.IsFalse(result);
            Assert.IsNull(policy);
        }
    }
}
