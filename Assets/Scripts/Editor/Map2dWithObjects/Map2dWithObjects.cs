using System.Linq;
using System;
using System.Collections.Generic;

namespace AStarGG.Test
{

/// <summary>
/// A map that may contain objects blocking the way of others
/// </summary>
/// <remarks>
/// In this example, Heros can pass through heroes.
/// Enemies can also pass through enemies.
/// Selecting as destination an occupied tile is not a verification extern to AStar.
/// </remarks>
public class Map2dWithObjects : IMap<TileSquared, int>
{
    public readonly List<TileSquared> Locations = new List<TileSquared>();
    public readonly HashSet<MapObject> Objects = new HashSet<MapObject>();

    TileSquared Find(int x, int y) =>
        Locations.FirstOrDefault(l => l.X == x && l.Y == y);

    public IEnumerable<TileSquared> NeighborsOf(TileSquared location, int flags)
    {
        List<TileSquared> neighbors = new List<TileSquared>();
        if(!Locations.Contains(location))
            return null;
        
        var coordinates = new List<Tuple<int, int>>();
        coordinates.Add(new Tuple<int, int>(location.X+1, location.Y));
        coordinates.Add(new Tuple<int, int>(location.X-1, location.Y));
        coordinates.Add(new Tuple<int, int>(location.X, location.Y+1));
        coordinates.Add(new Tuple<int, int>(location.X, location.Y-1));

        foreach(var coords in coordinates)
        {
            var loc = Find(coords.Item1, coords.Item2);
            if(loc != null)
                neighbors.Add(loc);
        }
        return neighbors;
    }

    public bool IsWalkable(TileSquared location, int flags) => Locations.Contains(location);
}
}