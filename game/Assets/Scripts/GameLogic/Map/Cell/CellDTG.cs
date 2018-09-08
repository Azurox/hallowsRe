using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellDTG : MonoBehaviour, IPointerClickHandler {
    public Cell currentCell;
    private List<CellColor> cellColors = new List<CellColor>();
    public bool active = false;

    public void SetCell(Cell cell)
    {
        active = false;
        currentCell = cell;
        UpdatePosition();
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

    private void UpdatePosition()
    {
        gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active) return;
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
