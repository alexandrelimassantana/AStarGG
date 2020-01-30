namespace AStarGG.Test
{

public class MapObject
{
    public static class CategoryId
    {
        public static readonly int Creature = 0x1;
        public static readonly int Hero = 0x2;
        public static readonly int Object = 0x4;
    }
    public readonly int Category;

    public MapObject(int category) => Category = category;
}

}