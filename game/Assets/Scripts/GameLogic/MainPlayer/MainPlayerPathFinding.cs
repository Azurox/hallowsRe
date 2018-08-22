using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerPathFinding : MonoBehaviour {
    private MapDTG mapDTG;
    private Cell[,] currentMap;
    public Node[,] grid;

    public List<Vector2> FindPath(int x, int y)
    {
        if(x == transform.position.x && y == transform.position.z)
        {
            return null;
        }

        if (mapDTG == null)
        {
            mapDTG = FindObjectOfType<MapDTG>();
            currentMap = mapDTG.GetMap().cells;
            grid = new Node[currentMap.GetLength(0), currentMap.GetLength(1)];

            for(var i = 0; i < currentMap.GetLength(0); i++)
            {
                for(var j = 0; j < currentMap.GetLength(1); j++)
                {
                    grid[i, j] = new Node(currentMap[j,i].IsAccessible, new Vector2(i, j), i, j);
                }
            }
        } else
        {
            for (var i = 0; i < currentMap.GetLength(0); i++)
            {
                for (var j = 0; j < currentMap.GetLength(1); j++)
                {
                    grid[i, j].parent = null;
                }
            }
        }

        Node startNode = grid[(int)transform.position.x, (int)transform.position.z];
        Node targetNode = grid[x, y];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return ReversePath(currentNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

        }

        return null;
    }


    public int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        int checkX;
        int checkY;

        // Haut 
        checkX = node.gridX - 1;
        checkY = node.gridY;
        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }


        //haut gauche
        checkX = node.gridX - 1;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }

        //gauche
        checkX = node.gridX;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }


        //bas gauche
        checkX = node.gridX + 1;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }


        //bas
        checkX = node.gridX + 1;
        checkY = node.gridY;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }


        //bas droite
        checkX = node.gridX + 1;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }

        //droite
        checkX = node.gridX;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }


        //haut droite
        checkX = node.gridX - 1;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbours.Add(grid[checkX, checkY]);
        }

        return neighbours;
    }

    public List<Vector2> ReversePath(Node currentNode)
    {
        List<Vector2> path = new List<Vector2>();
        var observedNode = currentNode;
        while(observedNode.parent != null && observedNode.parent.position != observedNode.position)
        {
            path.Add(observedNode.position);
            observedNode = observedNode.parent;
        }
        path.Reverse();
        return path;
    }
}
