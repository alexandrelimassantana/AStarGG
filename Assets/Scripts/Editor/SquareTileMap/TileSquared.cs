using System;

namespace AStarGG.Test
{

public class TileSquared : ILocation 
{
    public readonly int X, Y, Cost;

    public TileSquared(int x, int y, int cost = 1)
    {
        X = x;
        Y = y;
        Cost = cost;
    }

    public int DistanceEstimation(ILocation other)
    {
        if (other == null || GetType() != other.GetType())
            throw new InvalidOperationException("Cannot compare two locations with different implementations.");
        var otherLoc = (TileSquared) other;
        return Math.Abs(X - otherLoc.X) + Math.Abs(Y - otherLoc.Y);
    }
    
    public int MovementCost() => Cost;
}
}