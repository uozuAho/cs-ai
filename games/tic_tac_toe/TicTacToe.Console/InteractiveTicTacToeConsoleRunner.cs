﻿using System;
using System.Text;
using TicTacToe.Console.Test;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class InteractiveTicTacToeConsoleRunner
    {
        private readonly ITextInput _userInput;
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _register;

        public InteractiveTicTacToeConsoleRunner(ITextInput userInput, ITextOutput userOutput, PlayerRegister register)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _register = register;
        }

        public void Run(string[] args)
        {
            if (args.Length != 2 && args.Length != 3) throw new ArgumentException("must have 2 players");

            var player1 = _register.GetPlayerByName(args[0], BoardTile.X);
            var player2 = _register.GetPlayerByName(args[1], BoardTile.O);

            var numGames = args.Length == 3 ? int.Parse(args[2]) : 1;

            if (numGames <= 5)
            {
                RunInteractiveGames(numGames, player1, player2);
            }
            else
            {
                RunHeadless(numGames, player1, player2);
            }
        }

        private void ShowAvailablePlayers(PlayerRegister register)
        {
            Print("Player choices:");
            foreach (var description in register.AvailablePlayers())
            {
                Print("  " + description);
            }
        }

        private void RunHeadless(int numGames, ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            var runner = new HeadlessRunner(player1, player2);

            runner.PlayGames(numGames);

            var totalXWins = runner.NumberOfWins(BoardTile.X);
            var totalOWins = runner.NumberOfWins(BoardTile.O);
            Print($"After {runner.NumberOfGames} games, x wins, o wins: {totalXWins}, {totalOWins}");
        }

        private void RunInteractiveGames(int numGames, ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            for (var i = 0; i < numGames; i++)
            {
                RunSingleGame(player1, player2);
            }
        }

        private void RunSingleGame(ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            var game = new TicTacToeGame(Board.CreateEmptyBoard(), player1, player2);

            while (!game.IsFinished())
            {
                Print(RenderBoard(game.Board));
                game.DoNextTurn();
            }

            Print(RenderBoard(game.Board));

            Print($"The winner is: {game.Winner()}!");
        }

        private int PromptForNumberOfGames()
        {
            Print("How many games? (more than 5 runs headless)");
            return int.Parse(ReadLine());
        }

        private ITicTacToePlayer PromptForPlayer1(PlayerRegister register)
        {
            Print("Choose ticTacToePlayer 1 (x)");
            var input = ReadLine();
            return register.GetPlayerByKey(input, BoardTile.X);
        }

        private ITicTacToePlayer PromptForPlayer2(PlayerRegister register)
        {
            Print("Choose ticTacToePlayer 2 (o)");
            var input = ReadLine();
            return register.GetPlayerByKey(input, BoardTile.O);
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
        }

        private string ReadLine()
        {
            var line = _userInput.ReadLine();
            if (line == null) throw new InvalidOperationException("no!");
            return line;
        }

        private static string RenderBoard(Board board)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 9; i++)
            {
                var tile = board.GetTileAt(i);

                switch (tile)
                {
                    case BoardTile.Empty: sb.Append('.'); break;
                    case BoardTile.X: sb.Append('x'); break;
                    case BoardTile.O: sb.Append('o'); break;
                    default:
                        throw new InvalidOperationException();
                }

                if ((i + 1) % 3 == 0) sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}