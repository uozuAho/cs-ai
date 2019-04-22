using System.Collections.Generic;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class OrNode<TAction> : IAndOrTreeNode<TAction>
    {
        public IList<TAction> Actions { get; }
        
        public OrNode(TAction action)
        {
            Actions = new List<TAction> {action};
        }
    }
}