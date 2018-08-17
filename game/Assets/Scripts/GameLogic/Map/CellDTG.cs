using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDTG : MonoBehaviour {
    public Cell currentCell;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
        UpdatePosition();
        if (!cell.IsAccessible)
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0);
        }
    }

    private void UpdatePosition()
    {
        gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
    }
	
    public void OnMouseDown()
    {
        if (currentCell.IsAccessible)
        {
            transform.parent.gameObject.GetComponent<MapHandler>().TargetCell(currentCell.X, currentCell.Y);
        }else
        {
            Debug.Log("impossible to go here");
        }
    }
}
