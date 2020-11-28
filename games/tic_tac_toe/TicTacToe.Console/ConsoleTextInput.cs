namespace TicTacToe.Console
{
    public class ConsoleTextInput : ITextInput
    {
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}