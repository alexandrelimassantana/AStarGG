using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AstarGG.Structs;

namespace AStarGG
{
    public class Algorithm<T, Cookie> where T : class
    {
        struct Step
        {
            public T Parent;
            public int Cost;

            public Step(T parent, int cost)
            {
                Parent = parent;
                Cost = cost;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Update(T parent, int cost)
            {
                Parent = parent;
                Cost = cost;
            }
        }

        /// A representation of the world
        readonly IMap<T, Cookie> world;
        /// The nodes that are eligible for checking
        readonly Dictionary<T, Step> nodes;
        readonly MinHeap<T> minheap;
        /// A tree where root is the origin, branches are destinations and vertices are paths
        readonly PathTree<T> tree = new PathTree<T>();
        /// The current node being analyzed
        T current, destination;

        public Algorithm(IMap<T, Cookie> world)
        {
            this.world = world;
            nodes = new Dictionary<T, Step>();
            minheap = new MinHeap<T>((a,b) => Heuristic(b) - Heuristic(a));
        }

        public List<T> Calculate(T start, T end, Cookie c) 
        {
            SetUp(start, end);
            UpdateOrOpenNeighbor(c);
            while (!minheap.IsEmpty && !tree.Contains(end))
                MainLoop(c);
            return tree.PathTo(end); // Path or null if end not it tree
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetUp(T start, T dest = null)
        {
            nodes.Clear();
            minheap.Clear();
            tree.New(start);
            nodes[start] = new Step(null, 0);
            current = start;
            destination = dest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void UpdateOrOpenNeighbor(Cookie c)
        {
            var cost = nodes[current].Cost + world.MovementCost(current);
            foreach (var n in world.NeighborsOf(current, c))
            {
                // First time checking this node
                if (!tree.Contains(n) && !nodes.ContainsKey(n))
                {
                    nodes[n] = new Step(current, cost);
                    minheap.Push(n);
                }
                // Found better route to node
                else if (nodes[n].Cost > cost)
                {
                    if(tree.Contains(n))
                        tree.Nodes[n] = current;
                    nodes[n].Update(current, cost);
                    minheap.Sort();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MainLoop(Cookie c)
        {
            current = minheap.Pop();
            tree.Nodes[current] = nodes[current].Parent;
            UpdateOrOpenNeighbor(c);
        }


        /// Optimization criteria based on heuristics and the travel cost
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Heuristic(T n) => nodes[n].Cost + world.DistanceEstimation(n, destination);
    }
}