using System.Collections.Generic;

namespace TicTacToe.Console.Test
{
    public class StubUserInput : ITextInput
    {
        private readonly Queue<string> _lines = new Queue<string>();

        public void WillEnterLines(params string[] lines)
        {
            foreach (var line in lines)
            {
                _lines.Enqueue(line);
            }
        }

        public string ReadLine()
        {
            return _lines.Dequeue();
        }
    }
}