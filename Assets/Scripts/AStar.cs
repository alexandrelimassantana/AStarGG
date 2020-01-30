using System.Collections.Generic;
using System.Linq;
using AstarGG.Graph;

namespace AStarGG
{

public class Algorithm<T, Cookie> where T : class, ILocation
{
    /// The arguments of the AStar function
    public struct Arguments
    {
        /// Origin and Destination
        public T Origin, Destination;
        /// An Abstract representation of the World
        public IMap<T, Cookie> World;
        
        /// <summary>
        /// Implementation-defined cookie to query the world on certain parameters
        /// </summary>
        /// <remarks>
        /// This can be used so that the algorithm adapts into game-specific rules.
        /// An example would be a game with enemies (Team 0) and heroes (Team 1).
        /// When the flag is 0, enemies are moving, when 1 heroes are moving.
        /// When the Map receives "IsWalkable" it analyzes who is trying to walk.
        /// Thhen, it decides if a given tile can be occupied by an hero or/and an enemy.
        /// </remarks>
        public Cookie Cookie;
    }
    public Arguments Args;

    /// The private inner state of the algorithm
    struct State
    {
        /// The cost to travel to a given node from origin
        public Dictionary<NodeAnnotated<T>, int> G;

        /// The nodes that are eligible for checking
        public Dictionary<NodeAnnotated<T>, EdgeWeighted<T>> OpenNodes;
    
        /// A weighted tree which root it the origin and branches are paths
        public TreeWeighted<T> Tree;
        /// The node being evaluated currently
        public NodeAnnotated<T> CurrentNode;

        public void New()
        {
           G = new Dictionary<NodeAnnotated<T>, int>();
           //TOODO: Optimize this structure to avoid O(n) in every GetNext (min Heap)
           OpenNodes = new Dictionary<NodeAnnotated<T>, EdgeWeighted<T>>();
           Tree = new TreeWeighted<T>(); 
        }
        
        public void Clear()
        {
            G.Clear();
            OpenNodes.Clear();
            Tree.Clear();
            CurrentNode = null;
        }

        public void Set(Arguments args)
        {
            Clear();
            CurrentNode = new NodeAnnotated<T>(args.Origin);

            G[CurrentNode] = 0;
            Tree.Nodes.Add(CurrentNode);
            
            foreach(var neigh in args.World.NeighborsOf(CurrentNode.Label, args.Cookie))
            {
                var node = new NodeAnnotated<T>(neigh);
                G[node] = CurrentNode.Label.MovementCost();
                OpenNodes.Add(node, new EdgeWeighted<T>(CurrentNode, node));
            }
        }
    }
    State _state;

    public Algorithm() => _state.New();

    /// Heuristic function based on the implementation of ILocation
    int h(NodeAnnotated<T> node) => node.Label.DistanceEstimation(Args.Destination);
    /// Optimization criteria based on heuristics and the travel cost (g of a node)
    int f(NodeAnnotated<T> node) => _state.G[node] + h(node);

    NodeAnnotated<T> GetNodeForLocation(T location) =>
        _state.G.Keys.FirstOrDefault(n => n.Label.Equals(location));

    void UpdateOrOpenNeighbour(T neighbour)
    {
        var node = GetNodeForLocation(neighbour);
        var cost = _state.G[_state.CurrentNode] + _state.CurrentNode.Label.MovementCost();
        if(node == null) // First time checking this node
        {
            node = new NodeAnnotated<T>(neighbour);
            _state.OpenNodes.Add(node, new EdgeWeighted<T>(_state.CurrentNode, node));
            _state.G[node] = cost;
        }
        else if(_state.G[node] > cost) // Found better route to node
        {
            if(_state.Tree.Nodes.Contains(node))
                _state.Tree.UpdateParent(node, _state.CurrentNode);
            else
                _state.OpenNodes[node] = new EdgeWeighted<T>(_state.CurrentNode, node);
            _state.G[node] = cost;
        }
    }

    /// If there is at least one place to check and we are not in the destination
    bool SearchCriteria() =>
        _state.OpenNodes.Count != 0 && !_state.CurrentNode.Label.Equals(Args.Destination);
    public List<T> Calculate()
    {
        _state.Set(Args);

        if(Args.Destination.Equals(Args.Origin))
            return new List<T>{ Args.Origin };

        while(SearchCriteria())
        {
            var old = _state.CurrentNode;

            // Decide the next node to use
            _state.CurrentNode = _state.OpenNodes.First().Key;
            foreach(var n in _state.OpenNodes.Keys)
                if(f(_state.CurrentNode) > f(n))
                    _state.CurrentNode = n;

            // Add the current node to the Tree
            _state.Tree.Nodes.Add(_state.CurrentNode);
            _state.Tree.Edges.Add(_state.OpenNodes[_state.CurrentNode]);
            _state.OpenNodes.Remove(_state.CurrentNode);

            if(_state.CurrentNode.Label.Equals(Args.Destination))
                break;

            // Update all neighbours or insert them in the open list
            foreach(var neigh in Args.World.NeighborsOf(_state.CurrentNode.Label, Args.Cookie))
                UpdateOrOpenNeighbour(neigh);
        }

        // Destination was not found
        if(!_state.CurrentNode.Label.Equals(Args.Destination))
            return null;
        // Found a path
        var path = new List<T>();
        do
        {
            path.Add(_state.CurrentNode.Label);
            _state.CurrentNode = _state.Tree.GetParentNode(_state.CurrentNode);
        } while(!_state.CurrentNode.Label.Equals(Args.Origin));
        path.Add(Args.Origin);
        path.Reverse();
        return path;
    }
}

}