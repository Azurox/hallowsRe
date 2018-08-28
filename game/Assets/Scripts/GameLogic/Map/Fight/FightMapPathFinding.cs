using System;
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
        Init();
        var total = FindRange(currentMap[(int)startPosition.x, (int)startPosition.y], range);
        return total.ToList();
    }

    private HashSet<FightCellDTG> FindRange(FightCellDTG firstCell, int range)
    {
        if(range == 0)
        {
            return new HashSet<FightCellDTG>();
        }

        HashSet<FightCellDTG> firstHashset = new HashSet<FightCellDTG>
        {
            firstCell
        };

        HashSet<FightCellDTG> newCells = FindNeighbors(firstHashset);
        if(range == 1)
        {
            return newCells;
        }

        HashSet<FightCellDTG> totalCells = new HashSet<FightCellDTG>(newCells);

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


#region Spell region

    public List<Vector2> FindSpellRange(Vector2 startPosition, int range, bool includeStartPosition, bool inLine)
    {
        if(currentMap == null)
        {
            Init();
        }

        List<Vector2> rangeList = new List<Vector2>();
        for(int i = 0; i < currentMap.GetLength(0); i++)
        {
            for(int j = 0; j < currentMap.GetLength(1); j++)
            {
                var position = new Vector2(i, j);
                var totalVector = position - startPosition;
                var distance = System.Math.Abs(totalVector.x) + System.Math.Abs(totalVector.y);
                bool include = false;
                if (distance <= range && currentMap[i, j].currentCell.IsAccessible)
                {
                    if(includeStartPosition == false)
                    {
                        if (position != startPosition)
                        {
                            include = true;
                        }
                    }
                    else
                    {
                        include = true;
                    }

                    if (inLine)
                    {
                        if(!(totalVector.x == 0 || totalVector.y == 0))
                        {
                            include = false;
                        }
                    }
                }

                if (include)
                {
                    rangeList.Add(position);
                }
            }
        }
        return rangeList;
    }


    public List<Vector2> FindSpellRangeWithObstacle(Vector2 startPosition, List<Vector2> obstacles ,int range, bool includeStartPosition, bool inLine)
    {
        List<Vector2> rangeList = FindSpellRange(startPosition, range, includeStartPosition, inLine);
        List<Vector2> obstacleInRange = new List<Vector2>();
        foreach (var obs in obstacles)
        {
            var totalVector = obs - startPosition;
            var distance = System.Math.Abs(totalVector.x) + System.Math.Abs(totalVector.y);
            if(distance <= range)
            {
                Debug.Log("Add obstacle at X" + obs.x + " Y" + obs.y);
                obstacleInRange.Add(obs);
            }
        }

        foreach (var obs in obstacleInRange)
        {
            var magn = new Vector2(obs.x - startPosition.x, obs.y - startPosition.y);
            List<Vector2> cellAfterObs = new List<Vector2>();
            foreach (var cell in rangeList)
            {
                if(Vector2.Distance(startPosition, cell) > Vector2.Distance(startPosition, obs))
                {
                    cellAfterObs.Add(cell);
                    Debug.Log("Add cell after obs at X" + cell.x + " Y" + cell.y);
                }
            }


            List<Vector2> cellBehindObs = new List<Vector2>();
            foreach (var cell in cellAfterObs)
            {
                if (Vector2.Angle(new Vector2(obs.x - startPosition.x, obs.y - startPosition.y), new Vector2(cell.x - startPosition.x, cell.y - startPosition.y)) < 45 * Math.Pow(0.59034f, magn.magnitude - 1))
                {
                    rangeList.Remove(cell);
                    cellBehindObs.Add(cell);
                    Debug.Log("Add cell behind obs at X" + cell.x + " Y" + cell.y);
                    Debug.Log("With angle : " + Vector2.Angle(new Vector2(obs.x - startPosition.x, obs.y - startPosition.y), new Vector2(cell.x - startPosition.x, cell.y - startPosition.y)));
                    // Debug.Log("With magnitude : " + Vector2.Angle(new Vector2(obs.x - startPosition.x, obs.y - startPosition.y), new Vector2(cell.x - startPosition.x, cell.y - startPosition.y)));

                }
            }
/*
            foreach (var cell in cellBehindObs)
            {
                var distance = CalculateDistance(startPosition, obs, cell.x, cell.y);
                Debug.Log("distance for cell " + cell.x + " Y" + cell.y);
                Debug.Log(" distance : " + distance);
                if(distance <= 1 && !(Math.Abs(startPosition.x - cell.x) == Math.Abs(startPosition.y - cell.y)))
                {
                    rangeList.Remove(cell);
                }
            }*/
        }

        return rangeList;
    }

    private double CalculateDistance(Vector2 point1, Vector2 point2, double x0, double y0)
    {
        return ((Math.Abs((point2.y - point1.y) * x0 -
                         (point2.x - point1.x) * y0 +
                         point2.x * point1.y -
                         point2.y * point1.x)) /
                (Math.Pow((Math.Pow(point2.y - point1.y, 2) +
                           Math.Pow(point2.x - point1.x, 2)),
                          0.5)));
    }
    #endregion
}
