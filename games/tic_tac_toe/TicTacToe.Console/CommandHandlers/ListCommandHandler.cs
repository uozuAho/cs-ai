﻿using TicTacToe.Console.Io;

namespace TicTacToe.Console.CommandHandlers
{
    public class ListCommandHandler
    {
        private readonly PlayerRegister _register;
        private readonly ITextOutput _output;

        public ListCommandHandler(PlayerRegister register, ITextOutput output)
        {
            _register = register;
            _output = output;
        }

        public static ListCommandHandler Default()
        {
            return new(new PlayerRegister(), new ConsoleTextOutput());
        }

        public void Run()
        {
            _register.LoadPolicyFiles();

            foreach (var player in _register.AvailablePlayers())
            {
                _output.PrintLine(player);
            }
        }
    }
}