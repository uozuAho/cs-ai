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
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("must have at least one item");
            }
            var idx = random.Next(0, items.Count);
            return items[idx];
        }

        public static T Choice<T>(this Random random, IEnumerable<T> items)
        {
            return random.Choice(items.ToList());
        }
    }
}