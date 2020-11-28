using System.Collections.Generic;
using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TestUserOutput : ITextOutput
    {
        private readonly List<string> _capturedLines = new List<string>();
        private int _readIndex = 0;

        public void PrintLine(string line)
        {
            _capturedLines.Add(line);
        }

        public void ExpectLine(string line)
        {
            var actualLine = ReadLine();
            Assert.AreEqual(line, actualLine);
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
            while (!AllLinesRead())
            {
                ReadLine();
            }
        }

        public int NumberOfLines()
        {
            return _capturedLines.Count;
        }

        private bool AllLinesRead()
        {
            return _readIndex == _capturedLines.Count;
        }

        private string ReadLine()
        {
            return _capturedLines[_readIndex++];
        }
    }
}