using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightMapDTG : MonoBehaviour {

    private readonly Color BASE_COLOR = new Color(1, 1, 1);
    private readonly Color BLUE_COLOR = new Color(1, 0, 0);
    private readonly Color RED_COLOR = new Color(1, 0, 0);
    private readonly Color GREEN_COLOR = new Color(144/255f, 238/255f, 144/255f);



    private GameMap currentMap;
    private FightCellDTG[,] cells;
    public List<FightCellDTG> blueCells = new List<FightCellDTG>();
    public List<FightCellDTG> redCells = new List<FightCellDTG>();
    private List<Vector2> dirtySpawnCells = new List<Vector2>();
    private List<Vector2> dirtyMovementRangeCells = new List<Vector2>();


    public void Init()
    {
        currentMap = GetComponent<GlobalMapDTG>().GetMap();

        var oldCells = GetComponent<GlobalMapDTG>().GetCells();
      
        cells = new FightCellDTG[oldCells.GetLength(0), oldCells.GetLength(1)];


        for(var i = 0; i < oldCells.GetLength(0); i++)
        {
            for (var j = 0; j < oldCells.GetLength(1); j++)
            {
                    cells[i, j] = oldCells[i, j].GetComponent<FightCellDTG>();
            }
        }

        GetComponent<GlobalMapDTG>().ActivateFightCell();

    }

    public void SetSpawnCell(Side side, Vector2 position, bool taken)
    {
        SetCellAvailability(position, taken);
        if(side == Side.blue)
        {
            cells[(int)position.x, (int)position.y].AddColor(BLUE_COLOR, 20);
            blueCells.Add(cells[(int)position.x, (int)position.y]);
        }else
        {
            cells[(int)position.x, (int)position.y].AddColor(RED_COLOR, 20);
            redCells.Add(cells[(int)position.x, (int)position.y]);
        }
        dirtySpawnCells.Add(position);
    }

    public void SetCellAvailability(Vector2 position, bool taken)
    {
        cells[(int)position.x, (int)position.y].taken = taken;
    }

    public void ResetSpawnCells()
    {
        foreach (var cell in dirtySpawnCells.ToList()) // ToList is here to allow Remove while iterating on the list.
        {
            if(cells[(int)cell.x, (int)cell.y].GetComponent<Renderer>().material.color == BLUE_COLOR)
            {
                cells[(int)cell.x, (int)cell.y].RemoveColor(BLUE_COLOR);
                dirtySpawnCells.Remove(cell);
            }
            if (cells[(int)cell.x, (int)cell.y].GetComponent<Renderer>().material.color == RED_COLOR)
            {
                cells[(int)cell.x, (int)cell.y].RemoveColor(RED_COLOR);
                dirtySpawnCells.Remove(cell);
            }
        }
    }

    public void SetCellMovementColor(Vector2 position)
    {
        cells[(int)position.x, (int)position.y].GetComponent<Renderer>().material.color = GREEN_COLOR;
        dirtyMovementRangeCells.Add(position);
    }

    public void ResetMovementCell()
    {
        foreach (var cell in dirtyMovementRangeCells.ToList()) // ToList is here to allow Remove while iterating on the list.
        {
            cells[(int)cell.x, (int)cell.y].RemoveColor(GREEN_COLOR);
            dirtyMovementRangeCells.Remove(cell);
        }
    }

    public List<FightCellDTG> FindRange(Vector2 startPosition, int range)
    {
        var total = FindRange(cells[(int)startPosition.x, (int)startPosition.y], range);
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
            if (x + 1 < cells.GetLength(0) && cells[x + 1, y].currentCell.IsAccessible && !cells[x + 1, y].taken)
            {
                newCellDTGs.Add(cells[x + 1, y]);
            }
            if (x - 1 >= 0 && cells[x - 1, y].currentCell.IsAccessible && !cells[x-1,y].taken)
            {
                newCellDTGs.Add(cells[x - 1, y]);
            }
            if (y + 1 < cells.GetLength(1) && cells[x, y + 1].currentCell.IsAccessible && !cells[x,y+1].taken)
            {
                newCellDTGs.Add(cells[x, y + 1]);
            }
            if (y - 1 >= 0 && cells[x, y - 1].currentCell.IsAccessible && !cells[x, y-1].taken)
            {
                newCellDTGs.Add(cells[x, y - 1]);
            }
        }
        return newCellDTGs;
    }
}
