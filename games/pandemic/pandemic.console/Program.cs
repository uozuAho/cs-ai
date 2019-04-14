using System;

namespace pandemic.console
{
    class Program
    {
        static void Main(string[] args)
        {
//            var game = new PandemicStateMachine(new PandemicGameState(PandemicBoard.CreateRealGameBoard()), )

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
            Console.WriteLine("todo: report game state");
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
