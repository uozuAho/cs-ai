﻿using System.IO;
using TicTacToe.Agent;
using TicTacToe.Agent.Utils;

namespace TicTacToe.Console
{
    public class PolicyStorage
    {
        public void SaveJsonFile(string filename, BoardActionMap player)
        {
            File.WriteAllText(filename, "asdf");
        }
    }
}