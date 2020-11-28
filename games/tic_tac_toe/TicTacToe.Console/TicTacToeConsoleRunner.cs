using System;
using System.Text;
using TicTacToe.Console.Test;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class TicTacToeConsoleRunner
    {
        private readonly ITextInput _userInput;
        private readonly ITextOutput _userOutput;

        public TicTacToeConsoleRunner(ITextInput userInput, ITextOutput userOutput)
        {
            _userInput = userInput;
            _userOutput = userOutput;
        }

        public void Run()
        {
            var register = new PlayerRegister();
            ShowAvailablePlayers(register);

            var player1 = PromptForPlayer1(register);
            var player2 = PromptForPlayer2(register);

            var numGames = PromptForNumberOfGames();

            if (numGames <= 5)
            {
                RunInteractiveGames(numGames, player1, player2);
            }
            else
            {
                RunHeadless(numGames, player1, player2);
            }
        }

        private void RunHeadless(int numGames, IPlayer player1, IPlayer player2)
        {
            var runner = new HeadlessRunner(player1, player2);

            runner.PlayGames(numGames);

            var totalXWins = runner.NumberOfWins(BoardTile.X);
            var totalOWins = runner.NumberOfWins(BoardTile.O);
            Print($"After {runner.NumberOfGames} games, x wins, o wins: {totalXWins}, {totalOWins}");
        }

        private void RunInteractiveGames(int numGames, IPlayer player1, IPlayer player2)
        {
            for (var i = 0; i < numGames; i++)
            {
                RunSingleGame(player1, player2);
            }
        }

        private void RunSingleGame(IPlayer player1, IPlayer player2)
        {
            var game = new TicTacToeGame(new Board(), player1, player2);

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

        private IPlayer PromptForPlayer1(PlayerRegister register)
        {
            Print("Choose player 1 (x)");
            var input = ReadLine();
            return register.NewPlayer(input, BoardTile.X);
        }

        private IPlayer PromptForPlayer2(PlayerRegister register)
        {
            Print("Choose player 2 (o)");
            var input = ReadLine();
            return register.NewPlayer(input, BoardTile.O);
        }

        private void ShowAvailablePlayers(PlayerRegister register)
        {
            Print("Player choices:");
            foreach (var description in register.AvailablePlayers())
            {
                Print("  " + description);
            }
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
        }

        private string ReadLine()
        {
            return _userInput.ReadLine();
        }

        private static string RenderBoard(IBoard board)
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