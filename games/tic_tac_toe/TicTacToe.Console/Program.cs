using System.CommandLine;
using System.CommandLine.Invocation;

namespace TicTacToe.Console
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var root = new RootCommand
            {
                ListCommand(),
                PlayCommand(),
                TrainCommand()
            };

            return root.Invoke(args);
        }

        private static Command ListCommand()
        {
            return new("list", "List available players")
            {
                Handler = CommandHandler.Create(
                    () =>
                    {
                        ListCommandHandler.Default().Run();
                    })
            };
        }

        private static Command PlayCommand()
        {
            var command = new Command("play")
            {
                new Option<string>("--player1", "X player") {IsRequired = true},
                new Option<string>("--player2", "O player") {IsRequired = true},
                new Option<int>("--num-games",
                    getDefaultValue: () => 1,
                    "Number of games to play")
            };

            command.Handler = CommandHandler.Create<string, string, int>(
                (player1, player2, numGames) =>
                {
                    PlayCommandHandler.Default().Run(player1, player2, numGames);
                });

            return command;
        }

        private static Command TrainCommand()
        {
            var command = new Command("train")
            {
                new Option<string>(
                    "--agent-name",
                    "save trained policy with this name")
                {
                    IsRequired = true
                },

                new Option<string>(
                    "--opponent",
                    "opponent to train against")
                {
                    IsRequired = true
                }
            };

            command.Handler = CommandHandler.Create<string, string>(
                (agentName, opponent) =>
                {
                    TrainCommandHandler.Default().Run(opponent, agentName);
                });

            return command;
        }
    }
}
