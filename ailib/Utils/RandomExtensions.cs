using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Utils
{
    public static class RandomExtensions
    {
        public static bool TrueWithProbability(this Random random, double probability)
        {
            return random.NextDouble() < probability;
        }

        public static T Choice<T>(this Random random, IReadOnlyList<T> items)
        {
            var idx = random.Next(0, items.Count - 1);
            return items[idx];
        }

        public static T Choice<T>(this Random random, IEnumerable<T> items)
        {
            return random.Choice(items.ToList());
        }
    }
}