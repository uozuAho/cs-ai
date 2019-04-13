using System;

namespace ailib.DataStructures
{
    public class Edge
    {
        public readonly int From;
        public readonly int To;
        private double _weight;

        public Edge(int from, int to, double weight)
        {
            From = from;
            To = to;
            _weight = weight;
        }

        /** Returns the other end of the edge to the given end */
        public int Other(int n)
        {
            if (n == From) return To;
            if (n == To) return From;
            
            throw new ArgumentException();
        }
    }
}