using System;
using System.Runtime.CompilerServices;

namespace AstarGG.Structs
{
    public class MinHeap<T>
    {
        public delegate int Compare(T a, T b);

        #region State
        T[] raw; // Values
        readonly Compare compare; // Comparator
        int size, chunk; // Memory Attributes
        bool dirty; // Dirty bit for optimized subsequent additions

        #endregion

        public MinHeap(Compare cmp)
        {
            compare = cmp;
            Clean();
        }

        public MinHeap(Compare cmp, int reserve)
        {
            compare = cmp;
            Reserve(reserve);
        }

        #region Public Interface

        /// Get the amount of elements in the heap
        public int Count { get; private set; }

        /// Checks if there is at least one element
        public bool IsEmpty => Count == 0;

        /// Get the Min element
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("MinHeap: cannot access position");
            if (dirty)
                Sort();
            return raw[0];
        }

        /// Add an element to the heap
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T o)
        {
            if (Count == size)
            {
                Realloc(size + chunk);
                chunk <<= 1;
            }
            dirty = true;
            raw[Count++] = o;
        }

        /// Remove the Min element and does not reorganize
        /// This method should only be used if multiple edits in the heap are expected
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop()
        {
            var ret = Peek();
            raw[0] = raw[Count - 1];
            raw[Count - 1] = ret;
            Count--; // last element (former first) is not visible
            _Heapify(0); // Rearrange the root
            return ret;
        }

        /// Force a sort in the structure. Use this when updating values.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sort()
        {
            for (int i = Count / 2 - 1; i >= 0; i--)
                _Heapify(i);
            dirty = false;
        }

        /// Logically removes all elements from the heap
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clean()
        {
            Count = 0;
            chunk = 8;
            dirty = false;
        }

        /// Releases the memory holded by this structure
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Clean();
            raw = null;
            size = 0;
        }

        /// Guarantees that enough memory will be available to contain s entries
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(int s)
        {
            if (size < s)
                Realloc(s);
        }

        #endregion

        #region Private Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Realloc(int s)
        {
            var other = new T[s];
            if (Count > 0)
                Array.Copy(raw, other, Count);
            raw = other;
            size = s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void _Heapify(int root)
        {
            var l = 2 * root + 1;
            var r = 2 * root + 2;
            var min = root;

            if (l < Count && compare(raw[l], raw[min]) < 0)
                min = l;
            if (r < Count && compare(raw[r], raw[min]) < 0)
                min = r;

            if (min != root)
            {
                T swap = raw[root];
                raw[root] = raw[min];
                raw[min] = swap;
                _Heapify(min);
            }
        }
        #endregion
    }
}