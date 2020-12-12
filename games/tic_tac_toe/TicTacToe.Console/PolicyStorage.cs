using System.IO;
using TicTacToe.Agent;

namespace TicTacToe.Console
{
    public class PolicyStorage
    {
        public void SaveJsonFile(string filename, BoardActionMap player)
        {
            File.WriteAllText(filename, "asdf");
        }
    }
}