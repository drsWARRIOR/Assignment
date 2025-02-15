using UnityEngine;

// defines different type of enemy can be used in game
public enum ObstacleType
{
    Tree, Building, Spikes
}

// Creates a scriptable object asset
[CreateAssetMenu(fileName = "Obstacle", menuName = "Scriptable Objects/Obstacle")]
public class Obstacle : ScriptableObject
{
    public string _name; // name of the obstacle

    public ObstacleType _type; // type of obstacle

    public GameObject _prefab; //prefab of the obstacle
}
