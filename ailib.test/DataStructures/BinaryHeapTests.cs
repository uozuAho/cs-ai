using System.Collections.Generic;
using ailib.DataStructures;
using NUnit.Framework;

namespace ailib.test.DataStructures
{
    public class BinaryHeapTests
    {
        private BinaryHeap<int> h;

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
            h = new BinaryHeap<int>(new IntComparer());
        }

        [Test]
        public void Should_throw_on_emtpy_peek()
        {
            Assert.That(() => h.PeekMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_throw_on_emtpy_removes()
        {
            Assert.That(() => h.Remove(1), Throws.InvalidOperationException);
            Assert.That(() => h.RemoveMin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Should_have_0_size_when_empty()
        {
            Assert.AreEqual(0, h.Size);
        }

        [Test]
        public void Should_return_false_on_empty_contains()
        {
            Assert.IsFalse(h.Contains(32));
        }

        [Test]
        public void Should_return_false_on_empty_contains()
        {
            Assert.IsFalse(h.Contains(32));
        }

            it('add and remove single', function() {
                h.add(55);
                expect(h.contains(55)).toBe(true);
                expect(h.peekMin()).toBe(55);
                expect(h.size()).toBe(1);
                expect(h.removeMin()).toBe(55);
                expect(h.size()).toBe(0);
                expect(h.contains(55)).toBe(false);
            });

            it('add and remove sequence', function() {
                h.add(1);
                h.add(2);
                h.add(3);
                expect(h.removeMin()).toBe(1);
                expect(h.removeMin()).toBe(2);
                expect(h.removeMin()).toBe(3);
            });

            it('add and remove reverse sequence', function() {
                h.add(3);
                h.add(2);
                h.add(1);
                expect(h.removeMin()).toBe(1);
                expect(h.removeMin()).toBe(2);
                expect(h.removeMin()).toBe(3);
            });

            it('should throw on removing non-existent item', function() {
                h.add(3);
                expect(() => h.remove(4)).toThrow();
            });

            it('remove', function() {
                h.add(3);
                h.add(2);
                h.add(1);
                h.remove(2);
                expect(h.peekMin()).toBe(1);
                expect(h.size()).toBe(2);
            });
        });

    }
}
