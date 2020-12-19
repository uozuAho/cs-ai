﻿using System;
using System.Text;
using TicTacToe.Console.Test;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class InteractiveTicTacToeConsoleRunner
    {
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _register;

        public InteractiveTicTacToeConsoleRunner(ITextOutput userOutput, PlayerRegister register)
        {
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

            if (game.Winner().HasValue)
                Print($"The winner is: {game.Winner()}!");
            else
                Print("Draw!");
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
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