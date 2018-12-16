using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Cell currentCell;
    private List<CellColor> cellColors = new List<CellColor>();
    public bool active = true;

    public void Setup(Cell cell)
    {
        currentCell = cell;
        name = cell.Y + "-" + cell.X;
        gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
        cellColors.Clear();
        AddColor(new Color(1, 1, 1), 1);
        if (cell.IsAccessible) return;
        AddColor(cell.Obstacle ? new Color(0.3f, 0.3f, 0.3f) : new Color(0, 0, 0), 100);
    }

    public void AddColor(Color color, int priority)
    {
        cellColors.Add(new CellColor(color, priority));
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<Renderer>().material.color = cellColors.OrderByDescending(i => i.priority).FirstOrDefault().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active) return;
        if (currentCell.IsAccessible)
        {
            GetComponentInParent<MapComponent>().TargetCell(currentCell);
        }
        else
        {
            Debug.Log("Cell is taken");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!active) return;
        if (currentCell.IsAccessible)
        {
            GetComponentInParent<MapComponent>().MouseOver(currentCell);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!active) return;
        if (currentCell.IsAccessible)
        {
            GetComponentInParent<MapComponent>().MouseExit(currentCell);
        }
    }

    public bool IsAccessible()
    {
        return currentCell.IsAccessible;
    }

    public bool IsOffscreen()
    {
        return currentCell.OffScreen;
    }
}
