using System.Collections.Generic;
using ailib.Algorithms.Search;
using FakeItEasy;
using NUnit.Framework;

namespace ailib.test
{
    public class BreadthFirstSearchTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var problem = A.Fake<ISearchProblem<StateMock, ActionMock>>();
            var initialState = new StateMock("initial state");
            A.CallTo(() => problem.InitialState).Returns(initialState);
            A.CallTo(() => problem.GetActions(A<StateMock>._)).Returns(new List<ActionMock>());
            A.CallTo(() => problem.IsGoal(A<StateMock>._)).Returns(false);
            
            var bfs = new BreadthFirstSearch<StateMock, ActionMock>(problem);
            
            Assert.IsFalse(bfs.IsFinished);
            Assert.IsFalse(bfs.IsSolved);
            Assert.AreEqual(initialState, bfs.CurrentState);
        }
    }
    
    public class StateMock
    {
        public string Data { get; set; }

        public StateMock(string data)
        {
            Data = data;
        }
    }

    public class ActionMock
    {
        
    }
}
