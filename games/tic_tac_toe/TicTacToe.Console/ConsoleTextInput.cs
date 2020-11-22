namespace TicTacToe.Console
{
    public class ConsoleTextInput : ITextInput
    {
        public int GetInt()
        {
            return int.Parse(System.Console.ReadLine());
        }
    }
}