using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 position;
    public Node parent;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector2 _position, int _gridX, int _gridY)
    {
        walkable = _walkable;
        position = _position;
        gridX = _gridX;
        gridY = _gridY;
    }
}
