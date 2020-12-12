using System.IO;
using TicTacToe.Agent.MonteCarlo;

namespace TicTacToe.Console
{
    public class PolicyStorage
    {
        public void SaveJsonFile(string filename, TicTacToeMutablePolicy player)
        {
            File.WriteAllText(filename, "asdf");
        }
    }
}