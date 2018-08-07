using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDTG : MonoBehaviour {
    [SerializeField]
    private Cell currentCell;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        this.gameObject.transform.position = new Vector3(currentCell.X, 0, currentCell.Y);
    }
	
}
