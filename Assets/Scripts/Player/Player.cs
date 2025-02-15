using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles logic related to player
public class Player : MonoBehaviour
{
    public Vector2 playerPositionInGrid;

    [SerializeField]
    Grid grid;

    [SerializeField]
    private float speed = 10;

    public bool isMoving = false;

    [SerializeField]
    private Pathfinder pathfinder;


    void Start()
    {
        //Set the initial player position
        SetInitialPlayerPosition();
    }

    void Update()
    {
        //check if player is moving
        if (!isMoving)
        {
            //check if the selected cell is blocked
            if (grid.GetSpecificCell(grid.selected_cell_x, grid.selected_cell_y, grid.gridCells).isBlocked == true)
            {
                Debug.Log("The selected cell is blocked");
            }
            else
            {
                //start moving
                StartCoroutine(Move(grid.selected_cell_x, grid.selected_cell_y));
            }
        }
    }

    //set initial player position
    void SetInitialPlayerPosition()
    {
        int init_x, init_y;
        float world_x, world_y;
        int maxIterations = 100;
        int iteration = 0;

        do
        {
            //Generate a random coordinate
            init_x = Random.Range(0, grid.width);
            init_y = Random.Range(0, grid.height);
            iteration++;
        }
        while (grid.GetSpecificCell(init_x, init_y, grid.gridCells).isBlocked && iteration < maxIterations);

        //Converts grid coordinates into world coordinates
        world_x = grid.GetSpecificCell(init_x, init_y, grid.gridCells).transform.position.x;
        world_y = grid.GetSpecificCell(init_x, init_y, grid.gridCells).transform.position.z;

        playerPositionInGrid = new Vector2(init_x, init_y);// player position in grid coordinates

        //move the player based on world position
        transform.position = new Vector3(world_x, grid.groundOffset + (Cell.cellSize / 2), world_y);

        //update the selected cell
        grid.selected_cell_x = init_x;
        grid.selected_cell_y = init_y;
    }

    //Coroutine that moves the player along a path step by step
    IEnumerator Move(int dest_x, int dest_y)
    {
        isMoving = true;

        //Gets the path 
        List<Node> path = pathfinder.FindPath((int)playerPositionInGrid.x, (int)playerPositionInGrid.y, dest_x, dest_y, grid);

        // move along the path step by step
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(MoveToPoint(path[i].x, path[i].y));
        }

        isMoving = false;
    }

    //Coroutine that moves the player towards a specific point
    IEnumerator MoveToPoint(int index_x, int index_y)
    {
        Cell dest_Cell = grid.GetSpecificCell(index_x, index_y, grid.gridCells);

        //check if destination cell is valid and accessible
        if (dest_Cell == null || dest_Cell.isBlocked)
        {
            yield break;
        }

        //World coordinates of destination cell
        Vector3 targetPos = new Vector3(dest_Cell.transform.position.x, grid.groundOffset + (Cell.cellSize / 2),dest_Cell.transform.position.z);

        //move toward the destination cell
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        //update player's grid position
        playerPositionInGrid = new Vector2(index_x, index_y);
    }
}