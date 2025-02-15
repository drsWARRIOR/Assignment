public class Node
{
    public int x, y; // Coordinates of the node

    public bool isBlocked; // Flag indicates if node is accessible or not

    //g: cost from start node to current node
    //h: cost from current code to end target
    //f: total cost = g + h
    public float g, h, f;

    //reference to parent node
    public Node parent;

    //Constructor initialize all the members
    public Node(int x, int y, bool isBlocked)
    {
        this.x = x;
        this.y = y;
        this.isBlocked = isBlocked;
        g = 0;
        h = 0;
        f = 0;
        parent = null;
    }
}
