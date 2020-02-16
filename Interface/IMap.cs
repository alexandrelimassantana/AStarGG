using System.Collections.Generic;

namespace AStarGG
{
    
public interface IMap<T, Cookie> where T : class
{
    IEnumerable<T> NeighborsOf(T location, Cookie cookie);
    int DistanceEstimation(T a, T b);
    int MovementCost(T a);
}

}