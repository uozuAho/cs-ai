using ailib.Algorithms.Search.NonDeterministic;
using ailib.test.TestUtils;
using FakeItEasy;
using NUnit.Framework;

namespace ailib.test.Algorithms
{
    public class NonDeterministicDfsSearchTests
    {
        [Test]
        public void GetSolution_ReturnsAnAndOrTree()
        {
            var problem = A.Fake<INonDeterministicSearchProblem<StateMock, ActionMock>>();
            
            var dfsSearch = new NonDeterministicDfsSearch<StateMock, ActionMock>(problem);
            var solution = dfsSearch.GetSolution();
            
            Assert.IsNotNull(solution);
        }
    }
}