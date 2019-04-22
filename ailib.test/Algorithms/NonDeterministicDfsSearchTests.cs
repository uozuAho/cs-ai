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
            var problem = A.Fake<INonDeterministicSearchProblem<StateMock<int>, ActionMock<int>>>();
            
            var dfsSearch = new NonDeterministicDfsSearch<StateMock<int>, ActionMock<int>>(problem);
            var solution = dfsSearch.GetSolution();
            
            Assert.IsNotNull(solution);
        }
    }
}