using System.Collections.Generic;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface IAndOrTreeNode<TAction>
    {
        IList<TAction> Actions { get; }
    }
}