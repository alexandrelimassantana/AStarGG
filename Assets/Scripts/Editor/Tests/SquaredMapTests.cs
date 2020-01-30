using NUnit.Framework;

namespace AStarGG.Test
{
    public class SquaredMapTests
    {
        readonly MapSquare2D World = new MapSquare2D();
        readonly Algorithm<TileSquared, int> Algorithm = new Algorithm<TileSquared, int>();

 
        [SetUp]
        public void SetUp()
        {
            World.Locations.Clear();
            Algorithm.Args.World = World;
            Algorithm.Args.Cookie = 0;
        }

        [Test]
        public void OriginIsDestination()
        {
            World.Locations.Add(new TileSquared(0,0));
            Algorithm.Args.Origin = World.Locations[0];
            Algorithm.Args.Destination = World.Locations[0];
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == 1);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
        }

        [Test]
        public void OneStep()
        {
            World.Locations.Add(new TileSquared(0,0));
            World.Locations.Add(new TileSquared(0,1));

            Algorithm.Args.Origin = World.Locations[0];
            Algorithm.Args.Destination = World.Locations[1];
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == 2);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
            Assert.True(path[1].Equals(Algorithm.Args.Destination));
        }

        [Test]
        public void TwoSteps()
        {
            World.Locations.Add(new TileSquared(0,0));
            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(new TileSquared(0,2));

            Algorithm.Args.Origin = World.Locations[0];
            Algorithm.Args.Destination = World.Locations[2];
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == 3);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
            Assert.True(path[1].Equals(World.Locations[1]));
            Assert.True(path[2].Equals(Algorithm.Args.Destination));
        }

        [Test]
        public void CurvedPath()
        {
            World.Locations.Add(new TileSquared(0,0));
            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(new TileSquared(0,2));

            World.Locations.Add(new TileSquared(1,2));
            World.Locations.Add(new TileSquared(2,2));

            World.Locations.Add(new TileSquared(2,1));
            World.Locations.Add(new TileSquared(2,0));

            Algorithm.Args.Origin = World.Locations[0];
            Algorithm.Args.Destination = World.Locations[6];
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == World.Locations.Count);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
            Assert.True(path[6].Equals(Algorithm.Args.Destination));
        }

        [Test]
        public void ShortestCurvedPath()
        {
            World.Locations.Add(new TileSquared(0,0));
            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(new TileSquared(0,2));

            World.Locations.Add(new TileSquared(1,2));
            World.Locations.Add(new TileSquared(2,2));

            World.Locations.Add(new TileSquared(2,1));
            World.Locations.Add(new TileSquared(2,0));

            World.Locations.Add(new TileSquared(0,-1));
            World.Locations.Add(new TileSquared(1,-1));
            World.Locations.Add(new TileSquared(2,-1));

            Algorithm.Args.Origin = World.Locations[0];
            Algorithm.Args.Destination = World.Locations[6];
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == 5);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
            Assert.True(path[4].Equals(Algorithm.Args.Destination));
        }

        [Test]
        public void DeceptivePath()
        {
            var origin = new TileSquared(1,2);
            var destination = new TileSquared(3,2);
            for(int x = 0; x < 6; x++)
                World.Locations.Add(new TileSquared(x,0));

            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(new TileSquared(0,2));
            World.Locations.Add(origin);
            World.Locations.Add(new TileSquared(1,3));
            World.Locations.Add(new TileSquared(1,4));
            World.Locations.Add(new TileSquared(2,4));
            World.Locations.Add(new TileSquared(3,4));

            World.Locations.Add(new TileSquared(5,1));
            World.Locations.Add(new TileSquared(5,2));
            World.Locations.Add(new TileSquared(4,2));
            World.Locations.Add(destination);

            Algorithm.Args.Origin = origin;
            Algorithm.Args.Destination = destination;
            
            var path = Algorithm.Calculate();

            Assert.True(path.Count == 13);
            Assert.True(path[0].Equals(Algorithm.Args.Origin));
            Assert.True(path[12].Equals(Algorithm.Args.Destination));
        }
    }
}
