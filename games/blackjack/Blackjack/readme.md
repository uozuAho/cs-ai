# Blackjack

Incomplete. AI gym has a blackjack implementation, and I tested monte carlo RL
methods using that: https://github.com/uozuAho/rl_montecarlo_blackjack

This game can be formulated as an episodic, finite MDP:

- each game is an episode
- rewards at the end of each episode are win: +1, draw: 0, loss: -1
- actions: 'hit' or 'stay'
- assume cards are dealt from an infinite deck (ie. no use counting cards)
- dealer hits until their hand >= 17
