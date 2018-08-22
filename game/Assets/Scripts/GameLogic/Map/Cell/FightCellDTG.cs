using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCellDTG : MonoBehaviour {
    public Cell currentCell;
    public bool taken = false;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
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
}
