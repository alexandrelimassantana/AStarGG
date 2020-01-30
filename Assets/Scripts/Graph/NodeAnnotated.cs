namespace AstarGG.Graph
{
    public class NodeAnnotated<T>
    {
        public readonly T Label;
        public NodeAnnotated(T label) => Label = label;
    }
}