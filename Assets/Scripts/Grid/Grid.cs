using System.Collections.Generic;
using UnityEngine;

// Grid contains the data and implementation of function related to grid
public class Grid : MonoBehaviour
{
    [HideInInspector]
    public int selected_cell_x, selected_cell_y;

    [Header("Animation")]
    [SerializeField]
    [Range(0, 1)]
    private float upliftValue = 0.2f;

    [SerializeField]
    private float animationSpeed = 10f;

    [Header("Cell Setting")]

    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private GameObject blockedCellPrefab;

    [SerializeField, Range(0, 1)]
    private float celldistanceOffset;

    [Range(0, 2)]
    public float groundOffset;

    private Cell lastSelectedCell;
    private Vector3 originalPos;
    private bool isHovered = false;

    [HideInInspector]
    public List<GameObject> gridCells = new List<GameObject>();

    [Header("Grid Setting")]

    [SerializeField]
    private ArraySetup gridSetup;

    private ArraySetup lastGridSetup;

    [HideInInspector]
    public int width, height;

    [Header("Obstacles")]
    [SerializeField]
    private ObstacleManager obstacleManager;

    [HideInInspector]
    public List<GameObject> obstacles = new List<GameObject>();

    [Header("Pathfinding")]
    public Pathfinder pathfinder;

    [SerializeField]
    private Player player;

    private EnemyAI enemy;

    private void Awake()
    {
        //Initialize grid dimension based on the setup
        width = gridSetup.table.Length;
        height = width;

        //store the last know grid to detect changes
        lastGridSetup = CopyGridSetup(gridSetup);
        obstacleManager.lastObstacleData = CopyGridSetup(obstacleManager.obstacleData);

        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyAI>();

        // Renders the grid and initialize path finding nodes
        DrawGrid();
        pathfinder.InitNodes(this);
    }

    void Update()
    {
        //Redraws  the grid if any changes are detected in the grid
        if(!IsGridSetupSame(lastGridSetup, gridSetup) || !IsGridSetupSame(obstacleManager.lastObstacleData, obstacleManager.obstacleData))
        {
            lastGridSetup = CopyGridSetup(gridSetup);
            obstacleManager.lastObstacleData = CopyGridSetup(obstacleManager.obstacleData);
            DrawGrid();
        }

        // Handles cell selection
        CellSelect();
    }

    // Render the Grid
    void DrawGrid()
    {
        //Clear existing grid
        for (int i = 0; i < gridCells.Count; i++)
        {
            Destroy(gridCells[i]);
        }
        gridCells.Clear();

        //Clears existing obstacles
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacles.Clear();

        //Create new cells and obstacles based on grid setup
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject cell, obstacleObj;

                //Determine if the cell is blocked
                if (gridSetup.table[i].rows[j] == true)
                {
                    cell = Instantiate(blockedCellPrefab);
                    cell.GetComponent<Cell>().isBlocked = true;
                }
                else
                {
                    cell = Instantiate(cellPrefab);
                    cell.GetComponent<Cell>().isBlocked = false;
                }

                

                gridCells.Add(cell);

                // Set cell position and index
                cell.GetComponent<Cell>().index_x = i;
                cell.GetComponent<Cell>().index_y = j;
                float cellSize = Cell.cellSize;
                cell.transform.position = new Vector3(i * (cellSize + celldistanceOffset), groundOffset, j * (cellSize + celldistanceOffset));

                // Add obstacles if need
                if (obstacleManager.obstacleData.table[i].rows[j] == true)
                {
                    obstacleObj = Instantiate(obstacleManager.obstacle._prefab, cell.transform.position, Quaternion.identity);
                    cell.GetComponent<Cell>().isBlocked = true;
                    obstacles.Add(obstacleObj);
                    
                }
                else
                {
                    //Create an empty placeholder for non obstacle cells
                    obstacleObj = new GameObject();
                    Destroy(obstacleObj);
                    if (gridSetup.table[i].rows[j] == false)
                    {
                        cell.GetComponent<Cell>().isBlocked = false;
                    }
                }
            }
        }
    }

    // Handles all cell selection and animation effect
    void CellSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Initiates ray from camera
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cell cell = hit.collider.gameObject.GetComponent<Cell>();

            
            if (cell != null && cell != lastSelectedCell)
            {
                //reset the previous cell
                if (lastSelectedCell != null)
                {
                    lastSelectedCell.transform.position = originalPos;
                    lastSelectedCell.DisableUI();
                }

                //store the newly selected cell
                lastSelectedCell = cell;
                originalPos = cell.transform.position;
                isHovered = true;
            }

            // animate the hovered cell effect
            if (isHovered && lastSelectedCell != null) 
            {
                Vector3 newPos = originalPos + new Vector3(0, upliftValue, 0);
                lastSelectedCell.transform.position = Vector3.Lerp(lastSelectedCell.transform.position, newPos, animationSpeed * Time.deltaTime);
                lastSelectedCell.EnableUI();
            }

            // select the cell on mouse click if player is not moving
            if (Input.GetMouseButtonDown(0) && player.isMoving == false) 
            {
                selected_cell_x = cell.index_x;
                selected_cell_y = cell.index_y;
            }
        }
        else
        {
            // reset hover animation when the cursor is not pointing to the cell
            if (lastSelectedCell != null)
            {
                lastSelectedCell.transform.position = Vector3.Lerp(lastSelectedCell.transform.position, originalPos, animationSpeed * Time.deltaTime);
                lastSelectedCell.DisableUI();
            }
            isHovered = false;
        }
    }

    // Clones a grip setup
    ArraySetup CopyGridSetup(ArraySetup currentArray)
    {
        ArraySetup copiedArray = new ArraySetup();
        for (int i = 0; i < 10; i++)
        {
            if (copiedArray.table[i].rows == null || copiedArray.table[i].rows.Length != 10)
            {
                copiedArray.table[i].rows = new bool[10];
            }
            for (int j = 0; j < 10; j++)
            {
                copiedArray.table[i].rows[j] = currentArray.table[i].rows[j];
            }
        }

        return copiedArray;
    }

    // checks if two grid setup same or not
    bool IsGridSetupSame(ArraySetup setup_a, ArraySetup setup_b)
    {

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(setup_a.table[i].rows[j] != setup_b.table[i].rows[j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    //returns the cell with specific coordinates
    public Cell GetSpecificCell(int index_x,  int index_y, List<GameObject> gridCells)
    {
        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i].GetComponent<Cell>().index_x == index_x && gridCells[i].GetComponent<Cell>().index_y == index_y)
            {
                return gridCells[i].GetComponent<Cell>();
            }
        }

        Debug.Log($"{index_x},{index_y} cell not found");
        return null;
    }

    public bool IsCellOccupied(int index_x, int index_y)
    {
        Vector2 cellPos = new Vector2 (index_x, index_y);

        if(player != null && player.playerPositionInGrid == cellPos)
        {
            return true;
        }

        if(enemy != null && enemy.enemyPositionInGrid == cellPos)
        {
            return true;
        }

        return false;
    }
}
