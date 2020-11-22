using System;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class TicTacToeConsoleRunner
    {
        private ITextInput _userInput;

        public TicTacToeConsoleRunner(ITextInput userInput)
        {
            _userInput = userInput;
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

        private static void RunHeadless(int numGames, IPlayer player1, IPlayer player2)
        {
            const int batchSize = 1000;
            var numBatches = numGames / batchSize;
            var runner = new HeadlessRunner(player1, player2);

            for (var i = 0; i < numBatches; i++)
            {
                runner.PlayGames(batchSize);
                var xWins = runner.NumberOfWins(BoardTile.X, 100);
                var oWins = runner.NumberOfWins(BoardTile.O, 100);
                Print($"last {batchSize} games, x wins, o wins: {xWins}, {oWins}");
                Print("");
                Print("press a key to continue...");
                ReadLine();
            }
        }

        private static void RunInteractiveGames(int numGames, IPlayer player1, IPlayer player2)
        {
            for (var i = 0; i < numGames; i++)
            {
                RunSingleGame(player1, player2);
            }
        }

        private static void RunSingleGame(IPlayer player1, IPlayer player2)
        {
            var game = new TicTacToeGame(new Board(), player1, player2);

            while (!game.IsFinished())
            {
                Print(RenderBoard(game.Board));
                game.DoNextTurn();
            }

            Print(RenderBoard(game.Board));

            System.Console.WriteLine($"The winner is: {game.Winner()}!");
        }

        private static int PromptForNumberOfGames()
        {
            Print("How many games? (more than 5 runs headless)");
            return int.Parse(ReadLine());
        }

        private static IPlayer PromptForPlayer1(PlayerRegister register)
        {
            Print("Choose player 1 (x)");
            var input = ReadLine();
            return register.NewPlayer(input, BoardTile.X);
        }

        private static IPlayer PromptForPlayer2(PlayerRegister register)
        {
            Print("Choose player 2 (o)");
            var input = ReadLine();
            return register.NewPlayer(input, BoardTile.O);
        }

        private static void ShowAvailablePlayers(PlayerRegister register)
        {
            Print("Player choices:");
            foreach (var description in register.AvailablePlayers())
            {
                Print("  " + description);
            }
        }

        private static void Print(string message)
        {
            System.Console.WriteLine(message);
        }

        private static string ReadLine()
        {
            return System.Console.ReadLine();
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