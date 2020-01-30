using System.Collections.Generic;

namespace AStarGG
{
    
public interface IMap<T, Cookie> where T : class, ILocation
{
    IEnumerable<T> NeighborsOf(T location, Cookie cookie);
}

}