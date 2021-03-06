﻿using TicTacToe.Console.Io;

namespace TicTacToe.Console.CommandHandlers
{
    public class ListCommandHandler
    {
        private readonly PlayerRegister _register;
        private readonly LearningAgentRegister _agentRegister;
        private readonly ITextOutput _output;

        public ListCommandHandler(
            PlayerRegister register,
            LearningAgentRegister agentRegister,
            ITextOutput output)
        {
            _register = register;
            _agentRegister = agentRegister;
            _output = output;
        }

        public static ListCommandHandler Default()
        {
            return new(
                new PlayerRegister(),
                new LearningAgentRegister(),
                new ConsoleTextOutput()
            );
        }

        public void Run()
        {
            _register.LoadPolicyFiles();

            _output.PrintLine("Players:");
            foreach (var player in _register.AvailablePlayers())
            {
                _output.PrintLine($"  {player}");
            }

            _output.PrintLine("");
            _output.PrintLine("Learning agents:");
            foreach (var agent in _agentRegister.AvailableAgents())
            {
                _output.PrintLine($"  {agent}");
            }
        }
    }
}