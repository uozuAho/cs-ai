# Tic Tac Toe playground for RL agent testing/training

Run as a console app:

> cd TicTacToe.Console
> dotnet run

This will let you choose 2 players (including yourself), and the number
of games. There are currently 2 learning agents:


## RlBookPTableAgent

This agent creates a table of all possible game states, and the probability of
winning from each state. As it plays, it updates the table when it either wins/
loses from a given state.

This is implemented as per the example in the introduction of the [rl book], and
doesn't work. Maybe I've implemented it incorrectly.


## RlBookModifiedPTableAgent

Same as above, but actually improves win rate over time.


# todo

I want something like:

> dotnet run train policy.json  # trains the agent, saves data to policy
> dotnet run play policy.json  # plays the trained agent


# References

- [rl book](https://www.amazon.com/Reinforcement-Learning-Introduction-Adaptive-Computation-ebook/dp/B008H5Q8VA)