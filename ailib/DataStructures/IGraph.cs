using System.Collections.Generic;

namespace ailib.DataStructures
{
    // todo: IReadOnlyGraph for graph algorithms?
    public interface IGraph<T>
    {
        void AddNode(T node);
        void AddEdge(int from, int to);
        void AddEdge(int from, int to, double weight);
        
        IList<T> Nodes { get; }
        IEnumerable<T> Adjacent(int n);
    }
}