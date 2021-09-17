using System;
using pandemic.GameObjects;

namespace pandemic.console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var game = new PandemicGame();

            while (!game.IsFinished)
            {
                PromptUserForNextCommand();
                var move = ReadPlayerMoveFromConsole();
                game.DoMove(move);

                PrintGameStateToConsole(game);
            }
        }

        private static void PrintGameStateToConsole(PandemicGame game)
        {
        }

        private static PlayerMove ReadPlayerMoveFromConsole()
        {
            var input = Console.ReadLine();
            return new PlayerMove();
        }

        private static void PromptUserForNextCommand()
        {
            Console.WriteLine("what's your move?");
        }
    }
}
