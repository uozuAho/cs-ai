using ailib.Algorithms.Search.NonDeterministic;
using ailib.test.TestUtils;
using NUnit.Framework;

namespace ailib.test.Algorithms
{
    public class AndOrTreeTests
    {
        private AndOrTree<StateMock, ActionMock> _tree;

        [SetUp]
        public void Setup()
        {
            var root = new OrNode<ActionMock>(new ActionMock("dummy"));
            
            _tree = new AndOrTree<StateMock, ActionMock>(root);
        }
        
        [Test]
        public void GivenTreeWithOneOrNode_NextAction_ShouldReturnOrNodeAction()
        {
            var orNodeAction = new ActionMock("action 1");
            var orNode = new OrNode<ActionMock>(orNodeAction);
            
            _tree = new AndOrTree<StateMock, ActionMock>(orNode);

            // act
            var nextAction = _tree.NextAction(new StateMock("dummy"));
            
            // assert
            Assert.AreEqual(orNodeAction, nextAction);
        }
    }
}