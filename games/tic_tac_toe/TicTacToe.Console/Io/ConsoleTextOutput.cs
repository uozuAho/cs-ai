namespace TicTacToe.Console.Io
{
    public class ConsoleTextOutput : ITextOutput
    {
        public void PrintLine(string line)
        {
            System.Console.WriteLine(line);
        }
    }
}