using System;
using System.Collections.Generic;
using Blackjack;

namespace MonteCarlo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // I ended up doing this in python, see https://github.com/uozuAho/rl_montecarlo_blackjack

            /*
            def estimate_V(policy, gamma):
            V = { s: arbitrary for s in all_states}
            returns = { s:[] for s in all_states}

            while True:
                G_return = 0
                # generate episode using policy:
                episode = {
                    states: s0, s1, s2..., s(T - 1)
                    actions: a0, a1, a2..., a(T - 1)
                    rewards: r1, r2..., r(T - 1), rT
                }
                for t in [T - 1, T - 2, ... , 0]:
                    state = states[t]
            
                    G_return = gamma * G_return + rewards[t + 1]
                    if state not in states[0:t - 1]:  # <-- if first visit
                        returns[state].append(G_return)
                        V[state] = avg(returns[state])
            */
        }

        static void EvaluatePolicy()
        {
            var values = new Dictionary<BlackjackState, double>();
        }
    }
}
