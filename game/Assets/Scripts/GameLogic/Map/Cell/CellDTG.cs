using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellDTG : MonoBehaviour {
    public Cell currentCell;
    private List<CellColor> cellColors = new List<CellColor>();

    public void SetCell(Cell cell)
    {
        currentCell = cell;
        UpdatePosition();
        AddColor(new Color(1, 1, 1), 1);
        if (!cell.IsAccessible)
        {
            AddColor(new Color(0, 0, 0), 100);
        }
    }

    private void UpdatePosition()
    {
        gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
    }
	
    public void OnMouseDown()
    {
        if (!enabled) return;
        if (currentCell.IsAccessible)
        {
            transform.parent.GetComponent<MapHandler>().TargetCell(currentCell.X, currentCell.Y);
        }else
        {
            Debug.Log("impossible to go here");
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
