﻿using System;
using System.Linq;
using random_walk.Playground.mc;

namespace random_walk.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new RandomWalkEnvironment(5, 3);

            var mcEstimator = new RandomWalkMcValueEstimator();
            var mcEstimates = mcEstimator.Estimate(env);

            var formattedEstimates = mcEstimates.Select(e => $"{e:F}");
            Console.WriteLine($"mc estimates: {string.Join(",", formattedEstimates)}");
        }
    }
}
