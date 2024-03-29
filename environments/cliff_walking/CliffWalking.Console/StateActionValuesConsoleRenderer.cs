﻿using System.Linq;
using CliffWalking.Agent;
using CliffWalking.Agent.DataStructures;

namespace CliffWalking.Console
{
    internal class StateActionValuesConsoleRenderer
    {
        public static void RenderAverageValues(IStateActionValues values)
        {
            for (var y = 3; y >= 0; y--)
            {
                var vals = Enumerable.Range(0, 12)
                    .Select(x => RenderAverageValueAtPosition(values, new Position(x, y)));
                var line = string.Join(" ", vals);
                System.Console.WriteLine(line);
            }
        }

        public static void RenderHighestValues(IStateActionValues values)
        {
            for (var y = 3; y >= 0; y--)
            {
                var vals = Enumerable.Range(0, 12)
                    .Select(x => RenderHighestValueAtPosition(values, new Position(x, y)));
                var line = string.Join(" ", vals);
                System.Console.WriteLine(line);
            }
        }

        private static string RenderAverageValueAtPosition(IStateActionValues values, Position pos)
        {
            var posValues = values.ActionValues(pos).Select(av => av.Item2).ToList();
            if (posValues.Count == 0)
                return "      ";

            const string format = "{0:0000.0;-000.0}";
            return string.Format(format, posValues.Average());
        }

        private static string RenderHighestValueAtPosition(IStateActionValues values, Position pos)
        {
            var posValues = values.ActionValues(pos).Select(av => av.Item2).ToList();
            if (posValues.Count == 0)
                return "      ";

            const string format = "{0:0000.0;-000.0}";
            return string.Format(format, posValues.Max());
        }
    }
}