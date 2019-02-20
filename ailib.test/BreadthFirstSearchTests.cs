using System.Collections.Generic;
using System.Linq;
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
        public void Init()
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
        
        [Test]
        public void Solve_problem_with_no_solution_should_finish_and_be_not_solved()
        {
            var problem = A.Fake<ISearchProblem<StateMock, ActionMock>>();
            var initialState = new StateMock("initial state");
            A.CallTo(() => problem.InitialState).Returns(initialState);
            A.CallTo(() => problem.GetActions(A<StateMock>._)).Returns(new List<ActionMock>());
            A.CallTo(() => problem.IsGoal(A<StateMock>._)).Returns(false);
            
            var bfs = new BreadthFirstSearch<StateMock, ActionMock>(problem);
            bfs.Solve();
            
            Assert.IsTrue(bfs.IsFinished);
            Assert.IsFalse(bfs.IsSolved);
        }
        
        [Test]
        public void First_step()
        {
            var action1 = new ActionMock("action 1");
            var action2 = new ActionMock("action 2");
            var initialState = new StateMock("initial state");
            var state1 = new StateMock("state 1");
            var state2 = new StateMock("state 2");
            var problem = A.Fake<ISearchProblem<StateMock, ActionMock>>();
            A.CallTo(() => problem.InitialState).Returns(initialState);
            A.CallTo(() => problem.GetActions(A<StateMock>._)).Returns(new [] {action1, action2});
            A.CallTo(() => problem.DoAction(initialState, action1)).Returns(state1);
            A.CallTo(() => problem.DoAction(initialState, action2)).Returns(state2);
            
            var bfs = new BreadthFirstSearch<StateMock, ActionMock>(problem);
            bfs.Step();
            
            Assert.IsFalse(bfs.IsFinished);
        }
        
        [Test]
        public void Solve_should_solve_solvable_problem()
        {
            var action1 = new ActionMock("action 1");
            var action2 = new ActionMock("action 2");
            var initialState = new StateMock("initial state");
            var state1 = new StateMock("state 1");
            var state2 = new StateMock("state 2");
            var problem = A.Fake<ISearchProblem<StateMock, ActionMock>>();
            A.CallTo(() => problem.InitialState).Returns(initialState);
            A.CallTo(() => problem.GetActions(initialState)).Returns(new [] {action1});
            A.CallTo(() => problem.DoAction(initialState, action1)).Returns(state1);
            A.CallTo(() => problem.GetActions(state1)).Returns(new [] {action2});
            A.CallTo(() => problem.DoAction(state1, action2)).Returns(state2);
            A.CallTo(() => problem.IsGoal(state2)).Returns(true);
            
            var bfs = new BreadthFirstSearch<StateMock, ActionMock>(problem);
            bfs.Solve();
            
            Assert.IsTrue(bfs.IsFinished);
            Assert.IsTrue(bfs.IsSolved);

            var solution = bfs.GetSolutionTo(state2).ToList();
            Assert.AreEqual(new[] {action1, action2}, solution);
        }
    }
    
    public class StateMock
    {
        public string Data { get; }

        public StateMock(string data)
        {
            Data = data;
        }
        
        public override string ToString()
        {
            return Data;
        }
    }

    public class ActionMock
    {
        public string Data { get; }

        public ActionMock(string data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return Data;
        }
    }
}
