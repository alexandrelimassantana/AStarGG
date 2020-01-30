using System.Linq;
using System.Collections.Generic;

namespace AstarGG.Graph
{
    public class TreeWeighted<T>
    {
        public readonly HashSet<NodeAnnotated<T>> Nodes = new HashSet<NodeAnnotated<T>>();
        public readonly HashSet<EdgeWeighted<T>> Edges = new HashSet<EdgeWeighted<T>>();

        public void Clear()
        {
            Nodes.Clear();
            Edges.Clear();
        }

        public EdgeWeighted<T> GetParentEdge(NodeAnnotated<T> n) =>
            Edges.FirstOrDefault(e => e.Out.Equals(n));

        public NodeAnnotated<T> GetParentNode(NodeAnnotated<T> n) =>
            GetParentEdge(n)?.In;

        public void UpdateParent(NodeAnnotated<T> node, NodeAnnotated<T> newParent)
        {
            Edges.Remove(GetParentEdge(node));
            Edges.Add(new EdgeWeighted<T>(node, newParent));
        }
    }
}