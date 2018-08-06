using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDTG : MonoBehaviour {
    [SerializeField]
    private Cell currentCell;

    public void SetCell(Cell cell)
    {
        currentCell = cell;
    }
	
}
