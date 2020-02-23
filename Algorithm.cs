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
            minheap = new MinHeap<T>(null);
        }

        public List<T> PathToPoint(T start, T end, Cookie c, int cutoff = -1) 
        {
            SetUp(start, end);
            minheap.Comparator = (a,b) => Heuristic(b) - Heuristic(a);
            UpdateOrOpenNeighbor(c, cutoff);
            while (!minheap.IsEmpty && !tree.Contains(end))
                MainLoop(c, cutoff);
            return tree.PathTo(end); // Path or null if end not it tree
        }

        public PathTree<T> PathsFromPoint(T start, Cookie c, int cutoff = -1) 
        {
            SetUp(start);
            minheap.Comparator = (a,b) => nodes[b].Cost - nodes[a].Cost;
            UpdateOrOpenNeighbor(c, cutoff);
            while (!minheap.IsEmpty)
                MainLoop(c, cutoff);
            return tree;
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
        void UpdateOrOpenNeighbor(Cookie c, int cutoff)
        {
            var cost = nodes[current].Cost + world.MovementCost(current);
            // If cannot cover more distance than the cutoff
            if(cutoff >= 0 && cost > cutoff)
                return;
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
                        tree.ParentOf[n] = current;
                    nodes[n].Update(current, cost);
                    minheap.Sort();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MainLoop(Cookie c, int cutoff)
        {
            current = minheap.Pop();
            tree.ParentOf[current] = nodes[current].Parent;
            UpdateOrOpenNeighbor(c, cutoff);
        }


        /// Optimization criteria based on heuristics and the travel cost
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Heuristic(T n) => nodes[n].Cost + world.DistanceEstimation(n, destination);
    }
}