using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TicTacToe.Console.Io;

namespace TicTacToe.Console.Test.Utils
{
    public class TestUserOutput : ITextOutput
    {
        private readonly List<string> _capturedLines = new();
        private int _readIndex;

        public IEnumerable<string> Lines
        {
            get
            {
                ReadToEnd();
                return _capturedLines;
            }
        }

        public void PrintLine(string line)
        {
            foreach (var l in line.Split(Environment.NewLine))
            {
                _capturedLines.Add(l);
            }
        }

        public void ExpectLine(string line)
        {
            if (AllLinesRead())
            {
                Assert.Fail($"Expected at line {_readIndex + 1}: '{line}'. No more lines exist.");
            }

            Assert.AreEqual(line, ReadLine());
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

        public bool ContainsLine(Predicate<string> predicate)
        {
            ReadToEnd();
            return _capturedLines.Any(line => predicate(line));
        }

        private bool AllLinesRead()
        {
            return _readIndex == _capturedLines.Count;
        }

        private string ReadLine()
        {
            if (AllLinesRead()) throw new InvalidOperationException("No more output lines captured!");

            return _capturedLines[_readIndex++];
        }
    }
}