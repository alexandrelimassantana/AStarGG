using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AstarGG.Structs
{
    public class PathTree<T> where T : class
    {
        /// Nodes associated with their parent node
        public readonly Dictionary<T, T> ParentOf = new Dictionary<T, T>();

        public void New(T root)
        {
            ParentOf.Clear();
            ParentOf[root] = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T step) => ParentOf.ContainsKey(step);

        public List<T> PathTo(T destination)
        {
            if(!Contains(destination))
                return null;

            var path = new List<T>();
            var node = destination;
            do
                path.Add(node);
            while((node = ParentOf[node]) != null);
            path.Reverse();
            return path;
        }
    }
}