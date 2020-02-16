using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AstarGG.Structs
{
    public class PathTree<T> where T : class
    {
        /// Nodes associated with their parent node
        public readonly Dictionary<T, T> Nodes = new Dictionary<T, T>();

        public void New(T root)
        {
            Nodes.Clear();
            Nodes[root] = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T step) => Nodes.ContainsKey(step);

        public List<T> PathTo(T destination)
        {
            if(!Contains(destination))
                return null;

            var path = new List<T>();
            var node = destination;
            do
                path.Add(node);
            while((node = Nodes[node]) != null);
            path.Reverse();
            return path;
        }
    }
}