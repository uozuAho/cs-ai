using System.Collections.Generic;
using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TestUserOutput : ITextOutput
    {
        private readonly Queue<string> _expectedLines = new Queue<string>();
        private readonly Queue<string> _capturedLines = new Queue<string>();

        public void PrintLine(string line)
        {
        }

        // argh this is too much effort, just input all strings at start of test
        public void ExpectLine(string line)
        {
            Assert.AreEqual(line, _capturedLines.Dequeue());
        }

        public void ExpectLineContaining(string text)
        {
            Assert.True(_capturedLines.Dequeue().Contains(text));
        }

        public void ExpectLines(params string[] lines)
        {
        }

        public void IgnoreLinesUntil(string line)
        {
        }
    }
}