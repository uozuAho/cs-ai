namespace ailib.DataStructures
{
    public class UndirectedGraph<T> : Graph<T>
    {
        public void AddEdge(int from, int to, double weight = 1)
        {
            AddSingleEdge(from, to, weight);
            AddSingleEdge(to, from, weight);
        }
    }
}