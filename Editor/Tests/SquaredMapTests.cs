using NUnit.Framework;

namespace AStarGG.Test
{
    public class SquaredMapTests
    {
        readonly MapSquare2D World;
        readonly Algorithm<TileSquared, int> Algorithm;

        public SquaredMapTests()
        {
            World = new MapSquare2D();
            Algorithm = new Algorithm<TileSquared, int>(World);
        }
 
        [SetUp]
        public void SetUp() => World.Locations.Clear();

        [Test]
        public void OriginIsDestination()
        {
            var origin = new TileSquared(0,0);
            World.Locations.Add(origin);
            
            var path = Algorithm.PathToPoint(origin, origin, 0);

            Assert.True(path.Count == 1);
            Assert.True(path.Contains(origin));
        }

        [Test]
        public void OneStep()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(0,1);

            World.Locations.Add(origin);
            World.Locations.Add(dest);

            var path = Algorithm.PathToPoint(origin, dest, 0);

            Assert.True(path.Count == 2);
            Assert.True(path[0].Equals(origin));
            Assert.True(path[1].Equals(dest));
        }

        [Test]
        public void TwoSteps()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(0,2);
            
            World.Locations.Add(origin);
            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(dest);

            var path = Algorithm.PathToPoint(origin, dest, 0);

            Assert.True(path.Count == 3);
            Assert.True(path[0].Equals(origin));
            Assert.True(path[1].Equals(World.Locations[1]));
            Assert.True(path[2].Equals(dest));
        }

        [Test]
        public void CurvedPath()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(2,0);

            World.Locations.Add(origin);
            World.Locations.Add(new TileSquared(0,1));
            World.Locations.Add(new TileSquared(0,2));

            World.Locations.Add(new TileSquared(1,2));
            World.Locations.Add(new TileSquared(2,2));

            World.Locations.Add(new TileSquared(2,1));
            World.Locations.Add(dest);

            var path = Algorithm.PathToPoint(origin, dest, 0);

            Assert.True(path.Count == World.Locations.Count);
            Assert.True(path[0].Equals(origin));
            Assert.True(path[6].Equals(dest));
        }

        [Test]
        public void ShortestCurvedPath()
        {
            var origin = new TileSquared(0,1);
            var dest = new TileSquared(2,3);

            World.Locations.Add(new TileSquared(0,0));
            World.Locations.Add(origin);
            World.Locations.Add(new TileSquared(0,2));

            World.Locations.Add(new TileSquared(1,0));
            World.Locations.Add(new TileSquared(2,0));
            World.Locations.Add(new TileSquared(2,1));
            World.Locations.Add(new TileSquared(2,2));
            World.Locations.Add(dest);

            var path = Algorithm.PathToPoint(origin, dest, 0);

            Assert.True(path.Count == 7);
            Assert.True(path[0].Equals(origin));
            Assert.True(path[6].Equals(dest));
        }

        [Test]
        public void DeceptivePath()
        {
            var origin = new TileSquared(1,2);
            var dest = new TileSquared(3,2);
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
            World.Locations.Add(dest);

            var path = Algorithm.PathToPoint(origin, dest, 0);

            Assert.True(path.Count == 13);
            Assert.True(path[0].Equals(origin));
            Assert.True(path[12].Equals(dest));
        }

        [Test]
        public void NoCutoffFindsAllPaths()
        {
            var origin = new TileSquared(0,0);
            
            World.Locations.Add(origin);
            World.Locations.Add(new TileSquared(1,0));
            World.Locations.Add(new TileSquared(0,1));

            var paths = Algorithm.PathsFromPoint(origin, 0);

            foreach(var loc in World.Locations)
                Assert.Contains(loc, paths.ParentOf.Keys);
        }

        [Test]
        public void CutOffOnePath()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(1,0);

            World.Locations.Add(origin);
            World.Locations.Add(dest);
            World.Locations.Add(new TileSquared(2,0));

            var paths = Algorithm.PathsFromPoint(origin, 0, 1);

            Assert.Contains(origin, paths.ParentOf.Keys);
            Assert.True(paths.ParentOf.Count == 2);
            Assert.True(paths.PathTo(dest).Count == 2);
            Assert.True(paths.PathTo(dest)[0].Equals(origin));
        }

        [Test]
        public void CutOffTwoPaths()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(1,0);
            var dest2 = new TileSquared(0,1);

            World.Locations.Add(origin);
            World.Locations.Add(dest);
            World.Locations.Add(dest2);
            World.Locations.Add(new TileSquared(2,0));

            var paths = Algorithm.PathsFromPoint(origin, 0, 1);

            Assert.Contains(origin, paths.ParentOf.Keys);
            Assert.True(paths.ParentOf.Count == 3);
            Assert.True(paths.PathTo(dest).Count == 2);
            Assert.True(paths.PathTo(dest2).Count == 2);
        }

        [Test]
        public void CutOffBiggerThanOne()
        {
            var origin = new TileSquared(0,0);
            var dest = new TileSquared(2,0);

            World.Locations.Add(origin);
            World.Locations.Add(dest);
            World.Locations.Add(new TileSquared(1,0));

            var paths = Algorithm.PathsFromPoint(origin, 0, 2);

            Assert.Contains(origin, paths.ParentOf.Keys);
            Assert.True(paths.ParentOf.Count == 3);
            Assert.True(paths.PathTo(dest).Count == 3);
        }

        [Test]
        public void CutOffZero()
        {
            var origin = new TileSquared(0,0);
            World.Locations.Add(new TileSquared(1,0));
            World.Locations.Add(origin);

            var paths = Algorithm.PathsFromPoint(origin, 0, 0);

            Assert.Contains(origin, paths.ParentOf.Keys);
            Assert.True(paths.ParentOf.Count == 1);
        }
    }
}
