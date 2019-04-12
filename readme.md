# cs ai

Yet another ai library, this time using C#.

See other ai projects (todo: links)

# todo

- make binheap selectable min/max? may make priority queue, best first searches more intuitive

- vacuum world a*:
    - find consistent heuristic: h(n) <= c(n, a, n') + h(n') where
        c(n, a, n') is the cost of actions a that changes state from n to n'
    - show: every consistent h is admissable
    - find best possible heuristic: consistent, expands fewest possible nodes
    - document heuristic better: it is the estimated cost to reach the goal