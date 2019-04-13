using System.Collections.Generic;
using System.Linq;

namespace ailib.DataStructures
{
    public class UndirectedGraph<T> : Graph<T>, IGraph<T>
    {
        public void AddEdge(int from, int to)
        {
            AddEdge(from, to, 1);
        }

        public void AddEdge(int from, int to, double weight)
        {
            AddSingleEdge(from, to, weight);
            AddSingleEdge(to, from, weight);
        }

        public IList<T> Nodes => NodeList;
        
        public IEnumerable<T> Adjacent(int n)
        {
            return GetEdgesFrom(n).Select(e => NodeList[e.To]);
        }
    }
}