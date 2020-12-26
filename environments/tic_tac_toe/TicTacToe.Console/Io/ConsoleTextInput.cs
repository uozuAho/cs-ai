namespace TicTacToe.Console.Io
{
    public class ConsoleTextInput : ITextInput
    {
        public string? ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}