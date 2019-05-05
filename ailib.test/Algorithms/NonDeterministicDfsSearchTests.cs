using ailib.Algorithms.Search.NonDeterministic;
using ailib.test.TestUtils;
using FakeItEasy;
using NUnit.Framework;

namespace ailib.test.Algorithms
{
    public class NonDeterministicDfsSearchTests
    {
        private INonDeterministicSearchProblem<StateMock, ActionMock> _problem;
        private StateMock _initialState;

        [SetUp]
        public void Setup()
        {
            _problem = A.Fake<INonDeterministicSearchProblem<StateMock, ActionMock>>();
            _initialState = new StateMock("initial state");
        }
        
        [Test]
        public void GivenOnlyActionLeadsToGoal_FirstSolutionActionShouldGoToGoal()
        {
            var goalState = new StateMock("goal state");
            var goToGoal = new ActionMock("go to goal");

            A.CallTo(() => _problem.IsGoal(_initialState)).Returns(false);
            A.CallTo(() => _problem.IsGoal(goalState)).Returns(true);
            A.CallTo(() => _problem.GetActions(_initialState)).Returns(new [] {goToGoal});
            A.CallTo(() => _problem.DoAction(_initialState, goToGoal)).Returns(new[] {goalState});
            
            // act
//            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
//            var solution = dfsSearch.GetSolution();
            var solution = MyNonDetermDfsSearch<StateMock, ActionMock>.Search(_problem, _initialState);
            
            // assert
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
        }

        [Test]
        public void GivenNonDeterministicAction_ShouldReachGoal()
        {
            var state1 = new StateMock("state 1");
            var state2 = new StateMock("state 2");
            var goalState = new StateMock("goal state");
            var action = new ActionMock("action");

            A.CallTo(() => _problem.IsGoal(goalState)).Returns(true);
            // available actions
            A.CallTo(() => _problem.GetActions(_initialState)).Returns(new [] {action});
            A.CallTo(() => _problem.GetActions(state1)).Returns(new [] {action});
            A.CallTo(() => _problem.GetActions(state2)).Returns(new [] {action});
            // action outcomes
            A.CallTo(() => _problem.DoAction(_initialState, action)).Returns(new[]
            {
                state1,
                state2
            });
            A.CallTo(() => _problem.DoAction(state1, action)).Returns(new[] {goalState});
            A.CallTo(() => _problem.DoAction(state2, action)).Returns(new[] {goalState});

            // act
//            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
//            var solution = dfsSearch.GetSolution();
            var solution = MyNonDetermDfsSearch<StateMock, ActionMock>.Search(_problem, _initialState);
            
            // assert
            Assert.AreEqual(action, solution.NextAction(_initialState));
            Assert.AreEqual(action, solution.NextAction(state1));
        }
        
        [Test]
        public void GivenOneActionRepeatsAndOneGoesToGoal_ShouldReturnPlanToGoal()
        {
            var goalState = new StateMock("goal state");
            var repeat = new ActionMock("repeat");
            var goToGoal = new ActionMock("go to goal");

            A.CallTo(() => _problem.IsGoal(_initialState)).Returns(false);
            A.CallTo(() => _problem.IsGoal(goalState)).Returns(true);
            A.CallTo(() => _problem.GetActions(_initialState)).Returns(new [] {repeat, goToGoal});
            A.CallTo(() => _problem.DoAction(_initialState, repeat)).Returns(new[] {_initialState});
            A.CallTo(() => _problem.DoAction(_initialState, goToGoal)).Returns(new[] {goalState});
            
            // act
//            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
//            var solution = dfsSearch.GetSolution();
            var solution = MyNonDetermDfsSearch<StateMock, ActionMock>.Search(_problem, _initialState);
            
            // assert
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
        }

        [Test]
        public void GivenNonDeterministicActionAndRepeatedState_SolutionShouldKeepTryingToGoToGoal()
        {
            var goalState = new StateMock("goal state");
            var goToGoal = new ActionMock("go to goal");

            A.CallTo(() => _problem.IsGoal(goalState)).Returns(true);
            A.CallTo(() => _problem.GetActions(_initialState)).Returns(new [] {goToGoal});
            A.CallTo(() => _problem.DoAction(_initialState, goToGoal)).Returns(new[]
            {
                _initialState,
                goalState
            });

            // act
//            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
//            var solution = dfsSearch.GetSolution();
            var solution = MyNonDetermDfsSearch<StateMock, ActionMock>.Search(_problem, _initialState);
            
            // assert
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
        }
        
        [Test]
        public void GivenGoalIsTwoActionsAway_SolutionShouldLeadToGoal()
        {
            var nextState = new StateMock("next state");
            var goalState = new StateMock("goal state");
            var goToNext = new ActionMock("go to next");
            var goToGoal = new ActionMock("go to goal");

            A.CallTo(() => _problem.IsGoal(goalState)).Returns(true);
            A.CallTo(() => _problem.GetActions(_initialState)).Returns(new [] {goToNext});
            A.CallTo(() => _problem.GetActions(nextState)).Returns(new [] {goToGoal});
            A.CallTo(() => _problem.DoAction(_initialState, goToNext)).Returns(new[] {nextState});
            A.CallTo(() => _problem.DoAction(nextState, goToGoal)).Returns(new[] {goalState});

            // act
//            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
//            var solution = dfsSearch.GetSolution();
            var solution = MyNonDetermDfsSearch<StateMock, ActionMock>.Search(_problem, _initialState);
            
            // assert
            Assert.AreEqual(goToNext, solution.NextAction(_initialState));
            Assert.AreEqual(goToGoal, solution.NextAction(nextState));
        }
    }
}