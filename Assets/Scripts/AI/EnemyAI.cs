using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Implements AI behaviour for enemy character 
public class EnemyAI : MonoBehaviour,AI
{
    private Player player;
    private Vector2 enemyPositionInGrid;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private float moveSpeed;

    [HideInInspector]
    public bool isMoving = false;

    private Vector2 lastPlayerPos;

    void Awake()
    {
        //Find the player
        player = GameObject.FindWithTag("Player").GetComponent<Player>(); 
    }

    void Start()
    {
        //Set the initial enemy position
        SetInitialEnemyPosition();
    }

    void Update()
    {
        // if enemy is not moving and player changed the position, then it moves the enemy
        if (!isMoving && lastPlayerPos != player.playerPositionInGrid)
        {
            MoveTowardPlayer();
            lastPlayerPos = player.playerPositionInGrid;
        }
    }

    //Set initial enemy position
    void SetInitialEnemyPosition()
    {
        int init_x, init_y;
        float world_x, world_y;
        int maxIterations = 100;
        int iteration = 0;

        do
        {
            //spawn enemy at random cell
            init_x = Random.Range(0, grid.width);
            init_y = Random.Range(0, grid.height);

            //ensures enemy doesn't spawn at player's spawn position
            if(init_x == player.playerPositionInGrid.x && init_y == player.playerPositionInGrid.y)
            {
                continue;
            }
            iteration++;
        }
        while (grid.GetSpecificCell(init_x, init_y, grid.gridCells).isBlocked && iteration < maxIterations); // check if the cell is blocked or not

        //convert grid coordinates into world coordinates
        world_x = grid.GetSpecificCell(init_x, init_y, grid.gridCells).transform.position.x;
        world_y = grid.GetSpecificCell(init_x, init_y, grid.gridCells).transform.position.z;

        enemyPositionInGrid = new Vector2(init_x, init_y);

        //set enemy in word coordinates
        transform.position = new Vector3(world_x, grid.groundOffset + (Cell.cellSize / 2), world_y);
    }

    //Moves the enemy toward the player's position
    public void MoveTowardPlayer()
    {
        Vector2 playerPos = player.playerPositionInGrid;

        //possible adajacent cell direction of the player
        int[,] dir = new int[,] { { 0,  1}, { 1,  0}, { 0, -1}, {-1,  0}};

        float minDistance = float.MaxValue;
        int idle_x = (int)enemyPositionInGrid.x;
        int idle_y = (int)enemyPositionInGrid.y;

        for (int i = 0; i < dir.GetLength(0); i++)
        {
            int newCell_x = (int)playerPos.x + dir[i, 0];
            int newCell_y = (int)playerPos.y + dir[i, 1];

            //skip if out of boundaries
            if (newCell_x < 0 || newCell_x >= grid.width || newCell_y < 0 || newCell_y >= grid.height)
            {
                continue;
            }

            Cell cell = grid.GetSpecificCell(newCell_x, newCell_y, grid.gridCells);

            //skip if cell is blocked
            if (cell == null || cell.isBlocked)
            {
                continue;
            }

            // calculate distance to enemy
            float distance = Mathf.Sqrt((newCell_x - (int)enemyPositionInGrid.x) * (newCell_x - (int)enemyPositionInGrid.x) +
                                        (newCell_y - (int)enemyPositionInGrid.y) * (newCell_y - (int)enemyPositionInGrid.y));

            //select the cell with the shortest distance
            if (distance < minDistance)
            {
                minDistance = distance;
                idle_x = newCell_x;
                idle_y = newCell_y;
            }
        }

        //if movement is possible, get the path and move
        if (idle_x != (int)enemyPositionInGrid.x || idle_y != (int)enemyPositionInGrid.y)
        {
            List<Node> path = grid.pathfinder.FindPath((int)enemyPositionInGrid.x, (int)enemyPositionInGrid.y, idle_x, idle_y, grid);

            if (path != null && path.Count > 0)
            {
                StartCoroutine(Move(idle_x, idle_y, path));
            }
        }
    }

    //Coroutine to move along the path
    IEnumerator Move(int dest_x, int dest_y, List<Node> path)
    {
        isMoving = true;

        //move step by step along the path
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(MoveToPoint(path[i].x, path[i].y));
        }

        isMoving = false;
    }

    // Coroutine to move to a specific point
    IEnumerator MoveToPoint(int index_x, int index_y)
    {
        Cell dest_Cell = grid.GetSpecificCell(index_x, index_y, grid.gridCells);

        if (dest_Cell == null || dest_Cell.isBlocked)
            yield break;
        
        //converts to world coordinates
        Vector3 targetPos = new Vector3(dest_Cell.transform.position.x, grid.groundOffset + (Cell.cellSize / 2), dest_Cell.transform.position.z);

        //move towards the target position
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // update enemy position in the grid
        enemyPositionInGrid = new Vector2(index_x, index_y);
    }

}
