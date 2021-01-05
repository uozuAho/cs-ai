using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class PolicyFileLoaderTests
    {
        [Test]
        public void SaveAndLoad()
        {
            const string name = "name";
            const string description = "description";
            const string filename = "PolicyFileLoaderTests.SaveAndLoad.json";
            
            PolicyFileIo.Save(new SerializableStateActionPolicy(name, description, BoardTile.X), filename);
            var loaded = PolicyFileIo.FromFile("PolicyFileLoaderTests.SaveAndLoad.json");

            Assert.AreEqual(name, loaded.Name);
            Assert.AreEqual(description, loaded.Description);
        }
    }
}
