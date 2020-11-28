using TicTacToe.Console.Test;

namespace TicTacToe.Console
{
    public class ConsoleTextOutput : ITextOutput
    {
        public void PrintLine(string line)
        {
            System.Console.WriteLine(line);
        }
    }
}