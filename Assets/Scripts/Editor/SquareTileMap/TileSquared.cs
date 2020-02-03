using System;

namespace AStarGG.Test
{

public class TileSquared 
{
    public readonly int X, Y;

    public TileSquared(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int DistanceEstimation(TileSquared other)
    {
        if (other == null || GetType() != other.GetType())
            throw new InvalidOperationException("Cannot compare two locations with different implementations.");
        var otherLoc = (TileSquared) other;
        return Math.Abs(X - otherLoc.X) + Math.Abs(Y - otherLoc.Y);
    }
}
}