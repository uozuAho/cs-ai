# Tic Tac Toe playground for RL agent testing/training

Run as a console app:

```sh
cd TicTacToe.Console
# list available agents/players
dotnet run list
# train a monte carlo agent against a 'first available slot agent'
dotnet run train --agent mc --opponent FirstAvailableSlotAgent
# play a game against the trained agent
dotnet run play --player1 mc --player2 ConsoleInputTicTacToePlayer
```



# References

- [rl book](https://www.amazon.com/Reinforcement-Learning-Introduction-Adaptive-Computation-ebook/dp/B008H5Q8VA)
