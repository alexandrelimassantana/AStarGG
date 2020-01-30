namespace AStarGG
{

public interface ILocation
{
    int DistanceEstimation(ILocation location);
    int MovementCost();
}

}