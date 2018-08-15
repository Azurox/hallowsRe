using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDTG : MonoBehaviour {
    public Cell currentCell;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
    }
	
    public void OnMouseDown()
    {
        transform.parent.gameObject.GetComponent<MapHandler>().TargetCell(currentCell.X, currentCell.Y);
    }
}
