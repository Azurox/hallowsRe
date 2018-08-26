using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightMapPathFinding : MonoBehaviour {
    private FightMapDTG fightMapDTG;
    private FightCellDTG[,] currentMap;
    public Node[,] grid;


    // Use this for initialization
    public List<Vector2> FindPath(Vector2 from, Vector2 to)
    {

        Init();

        Node startNode = grid[(int)from.x, (int)from.y];
        Node targetNode = grid[(int)to.x, (int)to.y];

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


    private void Init()
    {
        if (fightMapDTG == null)
        {
            fightMapDTG = GetComponent<FightMapDTG>();
            currentMap = fightMapDTG.GetCells();
            grid = new Node[currentMap.GetLength(0), currentMap.GetLength(1)];

            for (var i = 0; i < currentMap.GetLength(0); i++)
            {
                for (var j = 0; j < currentMap.GetLength(1); j++)
                {
                    grid[i, j] = new Node(!currentMap[i, j].taken && currentMap[i, j].currentCell.IsAccessible, new Vector2(i, j), i, j);
                }
            }
        }
        else
        {
            for (var i = 0; i < currentMap.GetLength(0); i++)
            {
                for (var j = 0; j < currentMap.GetLength(1); j++)
                {
                    grid[i, j].parent = null;
                }
            }
        }
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

        //gauche
        checkX = node.gridX;
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

        //droite
        checkX = node.gridX;
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
        while (observedNode.parent != null && observedNode.parent.position != observedNode.position)
        {
            path.Add(observedNode.position);
            observedNode = observedNode.parent;
        }
        path.Reverse();
        return path;
    }

#region range Finding
    public List<FightCellDTG> FindRange(Vector2 startPosition, int range)
    {
        if(currentMap == null)
        {
            Init();
        }
        var total = FindRange(currentMap[(int)startPosition.x, (int)startPosition.y], range);
        return total.ToList();
    }

    private HashSet<FightCellDTG> FindRange(FightCellDTG firstCell, int range)
    {

        HashSet<FightCellDTG> firstHashset = new HashSet<FightCellDTG>
        {
            firstCell
        };

        HashSet<FightCellDTG> newCells = FindNeighbors(firstHashset);
        if(range == 1)
        {
            return newCells;
        }

        HashSet<FightCellDTG> totalCells = new HashSet<FightCellDTG>();

        for (int i = 0; i < range-1; i++)
        {
            newCells = FindNeighbors(newCells);
            totalCells.UnionWith(newCells);
        }

        return totalCells;
    }

    private HashSet<FightCellDTG> FindNeighbors (HashSet<FightCellDTG> oldCellDTGs)
    {
        HashSet<FightCellDTG> newCellDTGs = new HashSet<FightCellDTG>();
        foreach (var cell in oldCellDTGs)
        {
            int x = cell.currentCell.X;
            int y = cell.currentCell.Y;
            if (x + 1 < currentMap.GetLength(0) && currentMap[x + 1, y].currentCell.IsAccessible && !currentMap[x + 1, y].taken)
            {
                newCellDTGs.Add(currentMap[x + 1, y]);
            }
            if (x - 1 >= 0 && currentMap[x - 1, y].currentCell.IsAccessible && !currentMap[x-1,y].taken)
            {
                newCellDTGs.Add(currentMap[x - 1, y]);
            }
            if (y + 1 < currentMap.GetLength(1) && currentMap[x, y + 1].currentCell.IsAccessible && !currentMap[x,y+1].taken)
            {
                newCellDTGs.Add(currentMap[x, y + 1]);
            }
            if (y - 1 >= 0 && currentMap[x, y - 1].currentCell.IsAccessible && !currentMap[x, y-1].taken)
            {
                newCellDTGs.Add(currentMap[x, y - 1]);
            }
        }
        return newCellDTGs;
    }
    #endregion

}
