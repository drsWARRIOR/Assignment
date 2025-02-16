using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A Star path finding to calculate the path
public class Pathfinder : MonoBehaviour
{
    private Node[,] nodes; // 2d array of nodes to represent grid

    //Initialize nodes based grid, called in grid class
    public void InitNodes(Grid grid)
    {
        nodes = new Node[grid.width, grid.height];

        for (int i = 0; i < grid.gridCells.Count; i++)
        {
            Cell cell = grid.gridCells[i].GetComponent<Cell>();
            bool isBlocked = cell.isBlocked;
            nodes[cell.index_x, cell.index_y] = new Node(cell.index_x, cell.index_y, isBlocked);

        }
    }

    //returns the list of all valid adjacent nodes of given node
    List<Node> GetAdajacentNodes(Node node, Grid grid)
    {
        List<Node> adjacentNodes = new List<Node>();

        //Directions for all possible 4 direction
        int[,] dir = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        for (int i = 0; i < dir.GetLength(0); i++)
        {
            int x = node.x + dir[i, 0];
            int y = node.y + dir[i, 1];

            //check boundaries
            if (x >= 0 && x < grid.width && y >= 0 && y < grid.height)
            {
                //only adds if node is not blocked
                if (!nodes[x, y].isBlocked && !grid.IsCellOccupied(x,y))
                {
                    adjacentNodes.Add(nodes[x, y]);
                }
            }
        }

        return adjacentNodes;
    }

    //retraces the path from end node to the start node, returns list of nodes represting shortest path
    List<Node> RetracePath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    // calculates the manhattan distance between two nodes
    float GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // finds the shortest path from the start position to the goal position
    public List<Node> FindPath(int startX, int startY, int goalX, int goalY, Grid grid)
    {
        Node startNode = nodes[startX, startY];
        Node goalNode = nodes[goalX, goalY];

        List<Node> openList = new List<Node> { startNode }; // nodes to evaluate
        HashSet<Node> closedList = new HashSet<Node>(); //nodes evaulated

        while (openList.Count > 0)
        {
            // Select the node with the lowest f 
            Node currentNode = openList.OrderBy(n => n.f).ThenBy(n => n.h).First();
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // return to path, if reached the path
            if (currentNode == goalNode)
            {
                return RetracePath(startNode, goalNode);
            }

            //process all valid adajacent nodes
            List<Node> neighbors = GetAdajacentNodes(currentNode, grid);

            for (int i = 0; i < neighbors.Count; i++)
            {
                Node adjacent = neighbors[i];

                if (closedList.Contains(adjacent))
                    continue;

                float newMovementCost = currentNode.g + GetDistance(currentNode, adjacent);
                if (newMovementCost < adjacent.g || !openList.Contains(adjacent))
                {
                    adjacent.g = newMovementCost;
                    adjacent.h = GetDistance(adjacent, goalNode);
                    adjacent.f = adjacent.g + adjacent.h;
                    adjacent.parent = currentNode;

                    if (!openList.Contains(adjacent))
                        openList.Add(adjacent);
                }
            }
        }

        return null; // returns null if not path is found
    }

    //Draw the calculated the path on the grid, for debugging
    public void DrawPath(List<Node> path, Grid grid)
    {
        if (path == null)
        {
            Debug.Log("all directions are blocked");
            return;
        }
        else
        {
            for (int i = 0; i < path.Count; i++)
            {
                Node node = path[i];
                Cell cell = grid.GetSpecificCell(node.x, node.y, grid.gridCells);

                if (cell == null)
                {
                    Debug.LogWarning($"cell not found at ({node.x}, {node.y})");
                    continue;
                }


                Renderer renderer = cell.gameObject.GetComponent<Renderer>();



                renderer.material.color = Color.green;
            }
        }
    }

}
