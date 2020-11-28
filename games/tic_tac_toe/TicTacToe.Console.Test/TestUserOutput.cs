using System;
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
            foreach (var l in line.Split(Environment.NewLine))
            {
                _capturedLines.Add(l);
            }
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

        /// <summary>
        /// Expect the numbered line to equal the given string
        /// </summary>
        /// <param name="lineNumber">0-indexed line number. Negative reads from the end of all lines</param>
        /// <param name="line"></param>
        public void ExpectLine(int lineNumber, string line)
        {
            var positiveLineNumber = lineNumber < 0
                ? _capturedLines.Count + lineNumber
                : lineNumber;
            var actualLine = _capturedLines[positiveLineNumber];
            Assert.AreEqual(line, actualLine, $"at line {lineNumber}");
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