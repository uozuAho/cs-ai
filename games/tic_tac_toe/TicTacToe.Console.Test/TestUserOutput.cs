using System.Collections.Generic;
using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TestUserOutput : ITextOutput
    {
        private readonly Queue<string> _capturedLines = new Queue<string>();

        public void PrintLine(string line)
        {
            _capturedLines.Enqueue(line);
        }

        public void ExpectLine(string line)
        {
            Assert.AreEqual(line, _capturedLines.Dequeue());
        }

        public void ExpectLines(params string[] lines)
        {
            foreach (var line in lines)
            {
                ExpectLine(line);
            }
        }

        public void ReadToEnd()
        {
        }
    }
}