# Cliff walking

From example 6.6 in the RL book:

![](cliff_walking.png)

- 4x12 grid
- agent starts on bottom left (0, 0)
- goal is at bottom right (12, 0)
- agent can move one cell at a time
- reward = -1 for each step taken
- agent cannot step off the grid
- 10 cells at the middle bottom are 'the cliff'. If the agent
  goes here, it gets a reward of -100 and goes back to the start

`CliffWalking.Plots` compares interim & asymptotic performance of various agents
as their learning rates are varied:

![](cliff_walking_interim_vs_asymp.png)