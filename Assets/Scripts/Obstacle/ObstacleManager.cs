using UnityEngine;

//Manages the Obstacles
public class ObstacleManager : MonoBehaviour
{
    public Obstacle obstacle;

    [SerializeField]
    private string obstacleName;
    [SerializeField]
    private ObstacleType obstacleType;
    [SerializeField]
    private GameObject obstaclePrefab;

    [SerializeField]
    public ArraySetup obstacleData; //Stores the location of all obstacles

    [HideInInspector]
    public ArraySetup lastObstacleData; //Stores the last location of obstacles in case of any change made

    private void Awake()
    {
        //Initialize the obstacles properties based on the assigned value in editor
        obstacle._type = obstacleType; 
        obstacle.name = obstacleName;
        obstacle._prefab = obstaclePrefab;
    }

    private void Start()
    {
        // Initialize the 2D array that stores obstacle data
        for (int i = 0; i < 10; i++)
        {
            if (obstacleData.table[i].rows == null || obstacleData.table[i].rows.Length != 10)
            {
                obstacleData.table[i].rows = new bool[10];
            }
        }
    }
}
