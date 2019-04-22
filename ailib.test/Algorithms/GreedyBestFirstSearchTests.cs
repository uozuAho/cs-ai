using ailib.Algorithms.Search;
using ailib.test.TestUtils;
using FakeItEasy;
using NUnit.Framework;

namespace ailib.test.Algorithms
{
    public class GreedyBestFirstSearchTests
    {
        private ISearchProblem<StateMock, ActionMock<string>> _problem;
        private GreedyBestFirstSearch<StateMock, ActionMock<string>> _search;

        [SetUp]
        public void Setup()
        {
            _problem = A.Fake<ISearchProblem<StateMock, ActionMock<string>>>();
        }

        [Test]
        public void Step_ShouldGoToBetterState()
        {
            var state3 = new StateMock("3");
            var state2 = new StateMock("2");
            var state1 = new StateMock("1");

            var action1 = new ActionMock<string> {Value = "1"};
            var action3 = new ActionMock<string> {Value = "3"};
            
            A.CallTo(() => _problem.InitialState).Returns(state2);
            A.CallTo(() => _problem.GetActions(state2)).Returns(new[] {action1, action3});
            A.CallTo(() => _problem.DoAction(state2, action1)).Returns(state1);
            A.CallTo(() => _problem.DoAction(state2, action3)).Returns(state3);
            
            _search = new GreedyBestFirstSearch<StateMock, ActionMock<string>>(_problem, state => int.Parse(state.Value));
            
            // act
            _search.Step(); // step 1 sets current state to initial state
            _search.Step();
            
            // assert
            Assert.AreEqual(state1, _search.CurrentState);
        }
    }
}
