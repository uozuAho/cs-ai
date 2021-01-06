using System;
using System.IO;

namespace TicTacToe.Console.Test.Utils
{
    public class ConsoleCapture
    {
        private readonly TextWriter _captureStream;
        private readonly Stream _memoryStream;
        private readonly TestUserOutput _userOutput;
        private bool _isCaptureFinished;

        public ConsoleCapture()
        {
            _memoryStream = new MemoryStream();
            _captureStream = new StreamWriter(_memoryStream);
            _userOutput = new TestUserOutput();
            _isCaptureFinished = false;

            System.Console.SetOut(_captureStream);
        }

        public bool ContainsLine(Predicate<string> predicate)
        {
            FinishCapture();
            return _userOutput.ContainsLine(predicate);
        }

        private void FinishCapture()
        {
            if (_isCaptureFinished) return;

            TransferCaptureToUserOutput();

            _captureStream.Dispose();
            _memoryStream.Dispose();

            _isCaptureFinished = true;
        }

        private void TransferCaptureToUserOutput()
        {
            _captureStream.Flush();
            _memoryStream.Seek(0, SeekOrigin.Begin);
            using var sr = new StreamReader(_memoryStream);

            while (!sr.EndOfStream)
            {
                _userOutput.PrintLine(sr.ReadLine());
            }
        }
    }
}