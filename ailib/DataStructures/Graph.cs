using System;
using System.Collections.Generic;

namespace ailib.DataStructures
{
    public abstract class Graph<T>
    {
        protected readonly List<T> NodeList = new List<T>();

        private readonly List<List<Edge>> _adjacent = new List<List<Edge>>();

        public void AddNode(T node)
        {
            NodeList.Add(node);
            _adjacent.Add(new List<Edge>());
        }

        public List<Edge> GetEdgesFrom(int n)
        {
            ValidateIdx(n);
            return _adjacent[n];
        }

        protected void AddSingleEdge(int from, int to, double weight = 1)
        {
            ValidateIdx(from);
            ValidateIdx(to);
            _adjacent[from].Add(new Edge(from, to));
        }

        private void ValidateIdx(int n)
        {
            if (n < 0 || n > NodeList.Count - 1) throw new ArgumentOutOfRangeException();
        }
    }
}