using System.Collections.Generic;
using ailib.DataStructures;
using NUnit.Framework;

namespace ailib.test.DataStructures
{
    public class BinaryHeapTests
    {
        private BinaryMinHeap<int> _emptyIntMinHeap;

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
            _emptyIntMinHeap = new BinaryMinHeap<int>(new IntComparer());
        }

        [Test]
        public void Should_throw_on_empty_peek()
        {
            Assert.That(() => _emptyIntMinHeap.PeekMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_throw_on_empty_removes()
        {
            Assert.That(() => _emptyIntMinHeap.Remove(1), Throws.InvalidOperationException);
            Assert.That(() => _emptyIntMinHeap.RemoveMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_have_0_size_when_empty()
        {
            Assert.AreEqual(0, _emptyIntMinHeap.Size);
        }

        [Test]
        public void Should_return_false_on_empty_contains()
        {
            Assert.IsFalse(_emptyIntMinHeap.Contains(32));
        }

        [Test]
        public void Add_and_remove_single()
        {
            _emptyIntMinHeap.Add(55);
            Assert.IsTrue(_emptyIntMinHeap.Contains(55));
            Assert.AreEqual(55, _emptyIntMinHeap.PeekMin());
            Assert.AreEqual(1, _emptyIntMinHeap.Size);
            Assert.AreEqual(55, _emptyIntMinHeap.RemoveMin());
            Assert.AreEqual(0, _emptyIntMinHeap.Size);
            Assert.IsFalse(_emptyIntMinHeap.Contains(55));
        }

        [Test]
        public void Add_and_remove()
        {
            _emptyIntMinHeap.Add(1);
            _emptyIntMinHeap.Add(2);
            _emptyIntMinHeap.Add(3);
            Assert.AreEqual(1, _emptyIntMinHeap.RemoveMin());
            Assert.AreEqual(2, _emptyIntMinHeap.RemoveMin());
            Assert.AreEqual(3, _emptyIntMinHeap.RemoveMin());
        }

        [Test]
        public void Add_reverse_should_remove_in_priority_order()
        {
            _emptyIntMinHeap.Add(3);
            _emptyIntMinHeap.Add(2);
            _emptyIntMinHeap.Add(1);
            Assert.AreEqual(1, _emptyIntMinHeap.RemoveMin());
            Assert.AreEqual(2, _emptyIntMinHeap.RemoveMin());
            Assert.AreEqual(3, _emptyIntMinHeap.RemoveMin());
        }

        [Test]
        public void Remove_middle_item()
        {
            _emptyIntMinHeap.Add(1);
            _emptyIntMinHeap.Add(2);
            _emptyIntMinHeap.Add(3);
            _emptyIntMinHeap.Remove(2);
            Assert.AreEqual(1, _emptyIntMinHeap.PeekMin());
            Assert.AreEqual(2, _emptyIntMinHeap.Size);
        }
    }
}
