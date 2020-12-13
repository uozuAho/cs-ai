using System;
using System.Collections.Generic;

namespace TicTacToe.Console.Test.Utils
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
            if (_lines.Count == 0)
            {
                throw new InvalidOperationException("No input available");
            }
            return _lines.Dequeue();
        }
    }
}