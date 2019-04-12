using System.Collections.Generic;
using ailib.DataStructures;
using NUnit.Framework;

namespace ailib.test.DataStructures
{
    public class PriorityQueueTests
    {
        private MinPriorityQueue<int> _emptyIntQueue;

        private class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x < y ? -1 : x > y ? 1 : 0;
            }
        }
        
        [SetUp]
        public void Setup()
        {
            _emptyIntQueue = new MinPriorityQueue<int>(new IntComparer());
        }
        
        [Test]
        public void Pop_empty_should_throw()
        {
            Assert.That(() => _emptyIntQueue.Pop(), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Should_pop_in_descending_order()
        {
            _emptyIntQueue.Push(1);
            _emptyIntQueue.Push(5);
            _emptyIntQueue.Push(2);
            _emptyIntQueue.Push(4);
            _emptyIntQueue.Push(3);
            Assert.AreEqual(1, _emptyIntQueue.Pop());
            Assert.AreEqual(2, _emptyIntQueue.Pop());
            Assert.AreEqual(3, _emptyIntQueue.Pop());
            Assert.AreEqual(4, _emptyIntQueue.Pop());
            Assert.AreEqual(5, _emptyIntQueue.Pop());
        }
    }
}
