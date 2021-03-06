# Policy iteration

Using dynamic programming, find the optimal policy for a given problem. The
optimal policy is one that ensures the maximum possible reward from any starting
state.

Uses the same 'grid world' problem from
[policy evaluation](PolicyEvaluation.readme.md).

Policy improvement greedily selects the best action per state based on the
current value function:

$v_{\pi'}(s) = argmax(a) \sum_{s',r} p(s', r | s, a)[r + \gamma v_{\pi'}(s')]$

where argmax(a) means choose the a that yields the maximum of the given equation

An optimal policy can be found for a finite MDP by successively improving any
given policy.

For a deterministic policy, improve all actions:

```python
while policy_changed:
    for s in all_states:
        old_action = policy(s)
        new_action = find_max_value_action(s)
        policy(s) = new_action
        if old_action != new_action: policy_changed = True
    if not policy_changed: return policy
```

To find the optimal policy:

```python
while policy_changed:
    evaluate_policy(policy)
    improve_policy(policy)
```

In this example, we first choose a random policy and evaluate it. Then we pick a
greedy policy wrt the evaluated values, which finds the optimal policy
immediately.


# Run the example

Uncomment `GridWorldPolicyIteration.Run();` in `Program.cs` and run the project.
The example starts with a random policy, evaluates it, then creates a greedy
policy from the resulting value table. This finds an optimal policy in a single
step. Note that finding the optimal policy immediately isn't guaranteed, only an
improvement over the previous policy.
