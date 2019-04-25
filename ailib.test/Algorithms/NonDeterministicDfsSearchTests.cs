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
        public void GivenNoGoal_GetSolution_ShouldThrow()
        {
            A.CallTo(() => _problem.IsGoal(A<StateMock>._)).Returns(false);
            // don't set up actions = no available actions
            
            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);

            Assert.That(() => dfsSearch.GetSolution(), Throws.InvalidOperationException);
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
            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
            var solution = dfsSearch.GetSolution();
            
            // assert
            Assert.AreEqual(goToGoal, solution.NextAction(_initialState));
        }
        
        [Test]
        public void GivenNonDeterministicAction_SolutionShouldKeepTryingToGoToGoal()
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
            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
            var solution = dfsSearch.GetSolution();
            
            // assert
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
            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(_problem, _initialState);
            var solution = dfsSearch.GetSolution();
            
            // assert
            Assert.AreEqual(goToNext, solution.NextAction(_initialState));
            Assert.AreEqual(goToGoal, solution.NextAction(nextState));
        }
    }
}