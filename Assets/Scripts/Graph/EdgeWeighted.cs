namespace AstarGG.Graph
{
    public class EdgeWeighted<T>
    {
        public readonly NodeAnnotated<T> In, Out;
        public readonly int Weight;

        public EdgeWeighted(NodeAnnotated<T> _in, NodeAnnotated<T> _out, int cost = 0)
        {
            In = _in;
            Out = _out;
            Weight = cost;
        }
    }
}