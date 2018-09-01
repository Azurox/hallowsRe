﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightCellDTG : MonoBehaviour
{
    public Cell currentCell;
    private List<CellColor> cellColors = new List<CellColor>();
    public bool taken = false;
    [SerializeField]
    private bool active = false;
    private new bool enabled; 

    public void SetCell(Cell cell)
    {
        active = false;
        currentCell = cell;
        cellColors.Clear();
        AddColor(new Color(1, 1, 1), 1);
        if (!cell.IsAccessible)
        {
            if (cell.Obstacle)
            {
                AddColor(new Color(0.3f, 0.3f, 0.3f), 100);
            }
            else
            {
                AddColor(new Color(0, 0, 0), 100);
            }
        }
    }

    public void OnMouseDown()
    {
        if (!active) return;
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
        if (!active) return;
        if (currentCell.IsAccessible && taken == false)
        {
            transform.parent.GetComponent<FightMapHandler>().MouseOverCell(currentCell.X, currentCell.Y);
        }
    }

    public void OnMouseExit()
    {
        if (!active) return;
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

    public void SetState(bool active)
    {
        this.active = active;
    }

    public bool GetState()
    {
        return active;
    }
}
