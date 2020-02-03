using NUnit.Framework;

using AstarGG.Structs;

namespace AStarGG.Test
{
    public class MinHeapTests
    {
        readonly MinHeap<int> minHeap = new MinHeap<int>(((i, j) => i - j));

        [SetUp]
        public void SetUp() => minHeap.Clear();

        [Test]
        public void OneInstertion()
        {
            minHeap.Push(1);
            Assert.True(minHeap.Peek() == 1);
        }

        [Test]
        public void TwoInsertions()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            Assert.True(minHeap.Peek() == 0);
        }

        [Test]
        public void ThreeInsertions()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            minHeap.Push(2);
            Assert.True(minHeap.Peek() == 0);
        }

        [Test]
        public void Removal()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            minHeap.Push(2);
            Assert.True(minHeap.Pop() == 0);
            Assert.True(minHeap.Peek() == 1);
        }

        [Test]
        public void Reuse()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            minHeap.Pop();
            minHeap.Pop();

            minHeap.Push(2);
            Assert.True(minHeap.Peek() == 2); // No leftover from previous exec
            Assert.True(minHeap.Count == 1);
        }

        [Test]
        public void ReuseClean()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            minHeap.Clean();
            minHeap.Push(2);
            Assert.True(minHeap.Peek() == 2); // No leftover from previous exec
            Assert.True(minHeap.Count == 1);
        }

        [Test]
        public void ReuseClear()
        {
            minHeap.Push(1);
            minHeap.Push(0);
            minHeap.Clear();
            minHeap.Push(2);
            Assert.True(minHeap.Peek() == 2); // No leftover from previous exec
            Assert.True(minHeap.Count == 1);
        }

        [Test]
        public void MultipleSiftSort()
        {
            minHeap.Push(3);
            minHeap.Push(4);
            minHeap.Push(2);
            minHeap.Peek(); // Force a Sort() into [2,4,3]
            minHeap.Push(1); // Now elements are [2,4,3,1]

            // Sort(): [2,1,3,4] => [1,2,3,4] (Two sifts in one Sort loop)
            Assert.True(minHeap.Pop() == 1); // [1,2,3,4] => [4,2,3] => [2,4,3]
            Assert.True(minHeap.Pop() == 2); // [2,4,3] => [3,4]
            Assert.True(minHeap.Pop() == 3); // [3,4] => [4]
            Assert.True(minHeap.Pop() == 4); // [4] => []
        }

        [Test]
        public void MultipleSiftHeapifyLeft()
        {
            minHeap.Push(2);
            minHeap.Push(3);
            minHeap.Push(4);
            minHeap.Push(5);
            minHeap.Push(6);
            minHeap.Pop(); // Force a Sort() that does nothing [2,3,4,5,6]
            // Then swap 2 and 6 and removes 2 logically => [6,3,4,5]
            // 6, 3 and 5 are out of position
            // _Heapify(0): [6,3,4,5] => [3,6,4,5] (5 and 6 out of position)
            // _Heapify(1): [3,6,4,5] => [3,5,4,6] (two sifts on one pop)
            Assert.True(minHeap.Peek() == 3);
        }

        [Test]
        public void MultipleSiftHeapifyRight()
        {
            minHeap.Push(1);
            minHeap.Push(3);
            minHeap.Push(2);
            minHeap.Push(5);
            minHeap.Push(6);
            minHeap.Push(7);
            minHeap.Push(8);
            minHeap.Pop();
            Assert.True(minHeap.Peek() == 2);
        }
    }
}