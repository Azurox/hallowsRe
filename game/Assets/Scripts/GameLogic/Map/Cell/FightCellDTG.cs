using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightCellDTG : MonoBehaviour {
    public Cell currentCell;
    private List<CellColor> cellColors = new List<CellColor>();
    public bool taken = false;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
        AddColor(new Color(1, 1, 1), 1);
        if (!cell.IsAccessible)
        {
            AddColor(new Color(0, 0, 0), 10);
        }
    }

    public void OnMouseDown()
    {
        if (!enabled) return;
        if (currentCell.IsAccessible && taken == false)
        {
            transform.parent.GetComponent<FightMapHandler>().TargetCell(currentCell.X, currentCell.Y);
        }
        else
        {
            Debug.Log("Cell is taken");
        }
    }

    public void OnMouseEnter()
    {
        if (!enabled) return;
        if (currentCell.IsAccessible && taken == false)
        {
            transform.parent.GetComponent<FightMapHandler>().MouseOverCell(currentCell.X, currentCell.Y);
        }
    }

    public void OnMouseExit()
    {
        if (!enabled) return;
        if (currentCell.IsAccessible && taken == false)
        {
            transform.parent.GetComponent<FightMapHandler>().MouseLeftCell(currentCell.X, currentCell.Y);
        }
    }

    public void AddColor(Color color, int priority)
    {
        cellColors.Add(new CellColor(color, priority));
        SetColor();
    }

    public void RemoveColor(Color color)
    {
        foreach (var cellColor in cellColors.ToList())
        {
            if (cellColor.color == color)
            {
                cellColors.Remove(cellColor);
            }
        }
        SetColor();
    }

    private void SetColor()
    {
        GetComponent<Renderer>().material.color = cellColors.OrderByDescending(i => i.priority).FirstOrDefault().color;
    }

}
