# Finding the optimal policy for the gambler's problem

A gambler (our agent) can bet on the outcome of a coin toss. If the coin lands
heads up, the gambler wins the amount of money it bet on the toss. If the coin
lands tails up, the gambler loses the money it bet. The game ends when the
gambler has either $100 or $0.

This can be modelled as a finite, episodic MDP:
- state:   amount of cash held by the gambler {0-100}
- actions: amount of money to bet, or 'stake' on the toss {0 - min(s, 100 - s)}
- reward:  +1 when the gambler reaches $100, otherwise zero

When the probability of heads is known, the optimal policy can be found via DP
and policy or value iteration.

As before, the value of the current policy can be determined numerically:

![](img\policy_eval_successive_approx.png)

For the gambler's problem, the value update expands to:

```py
value = 0

for stake in 0..max_stake:
    prob_action = policy(stake, available_cash)
    value += prob_action * p(heads) * (reward + value(next_state(heads)))
           + prob_action * p(tails) * (reward + value(next_state(tails)))
```


# Run the example

Uncomment `GamblersProblemExample.Run();` in `Program.cs` and run the
project. You should see the values and an optimal policy printed out. Play around
with `probabilityOfHeads`, `dollarsToWin` and `evaluationSweepsPerPolicyUpdate`.

Notice that a high number of `evaluationSweepsPerPolicyUpdate` requires more
computation, and will not find a better policy that a lower number of sweeps.


# Notes

The RL book states that betting zero is a valid action, however I found that
when doing this, the optimal policy ended up betting zero for almost all states.

It turns out that for an unfair coin, an optimal policy is to bet as much as
possible. There are other optimal policies, but the general idea is to minimise
the total number of bets, since the probability of winning diminishes with the
total number of bets. For an unfair coin in your advantage, always betting $1 is
an optimal policy, since the probability of winning increases with the number of
coin flips.
