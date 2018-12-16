using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPathFinding : MonoBehaviour {
    private MapDTG mapDTG;
    private CellComponent[,] currentMap;
    public Node[,] grid;
    private bool ready = false;

    public bool IsReady()
    {
        return ready;
    }

    public void Refresh(CellComponent[,] cells)
    {
        currentMap = cells;
        grid = new Node[cells.GetLength(0), cells.GetLength(1)];

        for (var i = 0; i < cells.GetLength(0); i++)
        {
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                if(cells[j, i] == null)
                {
                    grid[i, j] = new Node(false, new Vector2(i, j), i, j);
                } else
                {
                    grid[i, j] = new Node(cells[j, i].IsAccessible(), new Vector2(i, j), i, j);
                }
            }
        }
    }

    private void CleanUp()
    {
        for (var i = 0; i < grid.GetLength(0); i++)
        {
            for (var j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].parent = null;
            }
        }
    }

    public List<Vector2> FindPath(Vector2 startPosition, Vector2 selectedPosition)
    {
        if (startPosition == selectedPosition)
        {
            return null;
        }

        Node startNode = grid[(int)startPosition.x, (int)startPosition.y];
        Node targetNode = grid[(int)selectedPosition.x, (int)selectedPosition.y];

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

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closeSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }

        }

        CleanUp();
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

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int checkX;
        int checkY;

        // Haut 
        checkX = node.gridX - 1;
        checkY = node.gridY;
        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }


        //haut gauche
        checkX = node.gridX - 1;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        //gauche
        checkX = node.gridX;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }


        //bas gauche
        checkX = node.gridX + 1;
        checkY = node.gridY - 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }


        //bas
        checkX = node.gridX + 1;
        checkY = node.gridY;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }


        //bas droite
        checkX = node.gridX + 1;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        //droite
        checkX = node.gridX;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }


        //haut droite
        checkX = node.gridX - 1;
        checkY = node.gridY + 1;

        if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        return neighbors;
    }

    public List<Vector2> ReversePath(Node currentNode)
    {
        List<Vector2> path = new List<Vector2>();
        var observedNode = currentNode;
        while (observedNode.parent != null && observedNode.parent.position != observedNode.position)
        {
            path.Add(observedNode.position);
            observedNode = observedNode.parent;
        }
        path.Reverse();
        CleanUp();
        return path;
    }

    public List<Vector2> FindAvailableCellInRange(Vector2 startPosition, int range)
    {

        List<Vector2> rangeList = new List<Vector2>();
        for (int i = 0; i < currentMap.GetLength(0); i++)
        {
            for (int j = 0; j < currentMap.GetLength(1); j++)
            {
                var position = new Vector2(i, j);
                var totalVector = position - startPosition;
                var distance = System.Math.Abs(totalVector.x) + System.Math.Abs(totalVector.y);
                if (currentMap[i, j] != null && distance <= range && !currentMap[i, j].IsOffscreen() && currentMap[i, j].IsAccessible())
                {
                    rangeList.Add(position);
                }
            }
        }
        return rangeList;
    }
}
