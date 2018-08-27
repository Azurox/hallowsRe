using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightMapDTG : MonoBehaviour {

    private readonly Color BASE_COLOR = new Color(1, 1, 1);
    private readonly Color BLUE_COLOR = new Color(0, 0, 1);
    private readonly Color RED_COLOR = new Color(1, 0, 0);
    private readonly Color GREEN_COLOR = new Color(144/255f, 238/255f, 144/255f);
    private readonly Color ORANGE_COLOR = new Color(255 / 255f, 207 / 255f, 158 / 255f);
    private readonly Color LIGHT_BLUE_COLOR = new Color(135 / 255f, 206 / 255f, 158 / 235f);



    private GameMap currentMap;
    private FightCellDTG[,] cells;
    public List<FightCellDTG> blueCells = new List<FightCellDTG>();
    public List<FightCellDTG> redCells = new List<FightCellDTG>();
    private List<Vector2> dirtySpawnCells = new List<Vector2>();
    private List<Vector2> dirtyMovementRangeCells = new List<Vector2>();
    private List<Vector2> dirtyPathCells = new List<Vector2>();
    private List<Vector2> dirtySpellRangeCells = new List<Vector2>();
    private List<Vector2> dirtySpellImpactCells = new List<Vector2>();

    private bool isPathHighlightingBlocked = false;



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

    public FightCellDTG[,] GetCells()
    {
        return cells;
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
        cells[(int)position.x, (int)position.y].AddColor(GREEN_COLOR, 30);
        dirtyMovementRangeCells.Add(position);
    }

    public void ResetMovementCells()
    {
        foreach (var cell in dirtyMovementRangeCells) // ToList is here to allow Remove while iterating on the list.
        {
            cells[(int)cell.x, (int)cell.y].RemoveColor(GREEN_COLOR);
        }
        dirtyMovementRangeCells.Clear();
    }

    public void HighlightPath(List<Vector2> positions)
    {
        if (isPathHighlightingBlocked)
        {
            return;
        }

        if (positions != null)
        {
            foreach (var position in positions)
            {
                cells[(int)position.x, (int)position.y].AddColor(ORANGE_COLOR, 30);
            }
            dirtyPathCells.AddRange(positions);
        }
    }

    public List<Vector2> GetHighlightedPath()
    {
        return dirtyPathCells;
    }

    public void ResetPathCells()
    {
        foreach (var cell in dirtyPathCells)
        {
            cells[(int)cell.x, (int)cell.y].RemoveColor(ORANGE_COLOR);
        }
        dirtyPathCells.Clear();
    }

    public void BlockPathHighlighting(bool blockIt)
    {
        Debug.Log("someone set the blocking to : " + blockIt);
        isPathHighlightingBlocked = blockIt;
    }


    public void HighlightSpellRange(List<Vector2> positions)
    {
        if (positions != null)
        {
            foreach (var position in positions)
            {
                cells[(int)position.x, (int)position.y].AddColor(LIGHT_BLUE_COLOR, 30);
            }
            dirtySpellRangeCells.AddRange(positions);
        }
    }

    public void ResetSpellRangeCells()
    {
        foreach (var cell in dirtySpellRangeCells)
        {
            cells[(int)cell.x, (int)cell.y].RemoveColor(LIGHT_BLUE_COLOR);
        }
        dirtySpellRangeCells.Clear();
    }

    public void HighlightSpellImpact(Vector2[] area, Vector2 position)
    {
        for(int i = 0; i < area.Length; i++)
        {
            Vector2 target = area[i] + position;
            if(target.x >= 0 && target.x < cells.GetLength(0)
                && target.y >= 0 && target.y < cells.GetLength(1))
            {
                cells[(int)target.x, (int)target.y].AddColor(RED_COLOR, 40);
                dirtySpellImpactCells.Add(target);
            }
        }
    }

    public void ResetSpellImpact()
    {
        foreach (var cell in dirtySpellImpactCells)
        {
            cells[(int)cell.x, (int)cell.y].RemoveColor(RED_COLOR);
        }
        dirtySpellImpactCells.Clear();
    }

    public bool SpellImpactIsInRange(Vector2 position)
    {
        bool inRange = false;
        foreach (var pos in dirtySpellRangeCells)
        {
            if(position == pos)
            {
                inRange = true;
            }
        }
        return inRange;
    }
}
