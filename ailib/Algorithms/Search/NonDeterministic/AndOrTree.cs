namespace ailib.Algorithms.Search.NonDeterministic
{
    public class AndOrTree<TState, TAction>
    {
        private IAndOrTreeNode<TAction> _root;

        public AndOrTree(IAndOrTreeNode<TAction> root)
        {
            _root = root;
        }

        public TAction NextAction(TState state)
        {
            return _root.Actions[0];
        }
    }
}