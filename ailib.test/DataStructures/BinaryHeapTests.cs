using System;
using System.Collections.Generic;
using ailib.DataStructures;
using NUnit.Framework;

namespace ailib.test.DataStructures
{
    public class BinaryHeapTests
    {
        private BinaryHeap<int> _emptyIntHeap;

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
            _emptyIntHeap = new BinaryHeap<int>(new IntComparer());
        }

        [Test]
        public void Should_throw_on_empty_peek()
        {
            Assert.That(() => _emptyIntHeap.PeekMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_throw_on_empty_removes()
        {
            Assert.That(() => _emptyIntHeap.Remove(1), Throws.InvalidOperationException);
            Assert.That(() => _emptyIntHeap.RemoveMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_have_0_size_when_empty()
        {
            Assert.AreEqual(0, _emptyIntHeap.Size);
        }

        [Test]
        public void Should_return_false_on_empty_contains()
        {
            Assert.IsFalse(_emptyIntHeap.Contains(32));
        }

        [Test]
        public void Add_and_remove_single()
        {
            _emptyIntHeap.Add(55);
            Assert.IsTrue(_emptyIntHeap.Contains(55));
            Assert.AreEqual(55, _emptyIntHeap.PeekMin());
            Assert.AreEqual(1, _emptyIntHeap.Size);
            Assert.AreEqual(55, _emptyIntHeap.RemoveMin());
            Assert.AreEqual(0, _emptyIntHeap.Size);
            Assert.IsFalse(_emptyIntHeap.Contains(55));
        }

        [Test]
        public void Add_and_remove()
        {
            _emptyIntHeap.Add(1);
            _emptyIntHeap.Add(2);
            _emptyIntHeap.Add(3);
            Assert.AreEqual(1, _emptyIntHeap.RemoveMin());
            Assert.AreEqual(2, _emptyIntHeap.RemoveMin());
            Assert.AreEqual(3, _emptyIntHeap.RemoveMin());
        }

        [Test]
        public void Add_reverse_should_remove_in_priority_order()
        {
            _emptyIntHeap.Add(3);
            _emptyIntHeap.Add(2);
            _emptyIntHeap.Add(1);
            Assert.AreEqual(1, _emptyIntHeap.RemoveMin());
            Assert.AreEqual(2, _emptyIntHeap.RemoveMin());
            Assert.AreEqual(3, _emptyIntHeap.RemoveMin());
        }

        [Test]
        public void Remove_middle_item()
        {
            _emptyIntHeap.Add(1);
            _emptyIntHeap.Add(2);
            _emptyIntHeap.Add(3);
            _emptyIntHeap.Remove(2);
            Assert.AreEqual(1, _emptyIntHeap.PeekMin());
            Assert.AreEqual(2, _emptyIntHeap.Size);
        }
    }
}
