﻿using TicTacToe.Agent.Storage;
using TicTacToe.Agent.Utils;
using TicTacToe.Console.Io;
using TicTacToe.Game;

namespace TicTacToe.Console.CommandHandlers
{
    public class TrainCommandHandler
    {
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _playerRegister;
        private readonly LearningAgentRegister _agentRegister;

        public TrainCommandHandler(
            ITextOutput userOutput,
            PlayerRegister playerRegister,
            LearningAgentRegister agentRegister)
        {
            _userOutput = userOutput;
            _playerRegister = playerRegister;
            _agentRegister = agentRegister;
        }

        public static TrainCommandHandler Default()
        {
            return new(
                new ConsoleTextOutput(),
                new PlayerRegister(),
                new LearningAgentRegister()
            );
        }

        public void Run(string agentName, string opponentName, int? numGamesLimit = null)
        {
            var agent = _agentRegister.GetAgentByName(agentName, BoardTile.X);

            _playerRegister.LoadPolicyFiles();

            if (!_playerRegister.TryFindPlayer(opponentName, BoardTile.O, out var opponent))
            {
                _userOutput.PrintLine($"Invalid opponent '{opponentName}'");
                return;
            }

            agent.Train(opponent!, numGamesLimit);
            agent.SaveTrainedValues(agentName, $"{agentName}.agent.json");

            _userOutput.PrintLine($"Trained agent '{agentName}' against '{opponentName}'");
        }
    }
}