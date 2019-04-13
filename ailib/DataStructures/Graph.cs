using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.DataStructures
{
    public abstract class Graph<T>
    {
        private readonly List<T> _nodes = new List<T>();

        private readonly List<List<Edge>> _adjacent = new List<List<Edge>>();

        public void AddNode(T node)
        {
            _nodes.Add(node);
            _adjacent.Add(new List<Edge>());
        }

        public List<Edge> GetEdgesFrom(int n)
        {
            return _adjacent[n];
        }

        protected void AddSingleEdge(int from, int to, double weight = 1)
        {
            ValidateIdx(from);
            ValidateIdx(to);
            _adjacent[from].Add(new Edge(from, to, weight));
        }

        private void ValidateIdx(int n)
        {
            if (n < 0 || n > _nodes.Count - 1) throw new ArgumentOutOfRangeException();
        }
    }
}