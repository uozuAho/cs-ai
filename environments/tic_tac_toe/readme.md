# Tic Tac Toe playground for RL agent testing/training

Run as a console app:

```sh
cd TicTacToe.Console
# list available agents/players
dotnet run list
# train a monte carlo agent against a 'first available slot agent', name it 'john'
dotnet run train mc FirstAvailableSlotAgent john
```

Some agents I made earlier:

## RlBookPTableAgent

This agent creates a table of all possible game states, and the probability of
winning from each state. As it plays, it updates the table when it either wins/
loses from a given state.

This is implemented as per the example in the introduction of the [rl book], and
doesn't work. Maybe I've implemented it incorrectly.


## RlBookModifiedPTableAgent

Same as above, but actually improves win rate over time.


## Td0Agent

One-step temporal difference (TD) learning agent, using on-policy learning.
Estimates the afterstate-value function.


# References

- [rl book](https://www.amazon.com/Reinforcement-Learning-Introduction-Adaptive-Computation-ebook/dp/B008H5Q8VA)
