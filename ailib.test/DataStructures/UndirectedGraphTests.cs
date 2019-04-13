using System.Linq;
using ailib.DataStructures;
using NUnit.Framework;

namespace ailib.test.DataStructures
{
    public class UndirectedGraphTests
    {
        private UndirectedGraph<int> _graph;

        [SetUp]
        public void Setup()
        {
            _graph = new UndirectedGraph<int>();
        }
        
        [Test]
        public void GivenASingleEdgeIsAdded_ShouldHaveOneEdge()
        {
            // arrange
            _graph.AddNode(0);
            _graph.AddNode(0);
            
            // act
            _graph.AddEdge(0, 1);

            // assert
            var edgesFrom0 = _graph.GetEdgesFrom(0);
            var edgesFrom1 = _graph.GetEdgesFrom(1);
            
            Assert.AreEqual(1, edgesFrom0.Count);
            Assert.AreEqual(0, edgesFrom0.Single().From);
            Assert.AreEqual(1, edgesFrom0.Single().To);
            
            Assert.AreEqual(1, edgesFrom1.Count);
            Assert.AreEqual(1, edgesFrom1.Single().From);
            Assert.AreEqual(0, edgesFrom1.Single().To);
        }
        
        [Test]
        public void CanAddDuplicateEdges()
        {
            // arrange
            _graph.AddNode(0);
            _graph.AddNode(0);
            
            // act
            _graph.AddEdge(0, 1);
            _graph.AddEdge(0, 1);

            // assert
            var edgesFrom0 = _graph.GetEdgesFrom(0);
            Assert.AreEqual(2, edgesFrom0.Count);
        }
        
        [Test]
        public void Adjacent()
        {
            // arrange
            _graph.AddNode(0);
            _graph.AddNode(1);
            _graph.AddEdge(0, 1);
            
            // act
            var adjacentTo0 = _graph.Adjacent(0).ToList();

            // assert
            Assert.AreEqual(1, adjacentTo0.Count);
            Assert.AreEqual(1, adjacentTo0.Single());
        }
    }
}