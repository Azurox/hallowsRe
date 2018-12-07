using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
    /* Linked GameObject */
    public CellComponent CellComponent;

    private CellComponent[,] cells;


    public void Setup(GameMap map)
    {
        if (cells == null)
        {
            cells = new CellComponent[map.cells.GetLength(0), map.cells.GetLength(1)];
        }

        for (int i = 0, length = map.cells.GetLength(0); i < length; i++)
        {
            for (int j = 0, lengthJ = map.cells.GetLength(1); j < lengthJ; j++)
            {
                if (map.cells[i, j].OffScreen) continue;

                CellComponent cell;
                if (cells[j, i] == null)
                {
                    cell = Instantiate(CellComponent, transform);
                    cells[j, i] = cell;
                }
                else
                {
                    cell = cells[j, i];
                }

                cell.Setup(map.cells[i, j]);
            }
        }
    }

    public void TargetCell(Cell cell)
    {
        // Debug.Log( $"click on cell {cell.X} : {cell.Y}");
    }

    public void MouseOver(Cell cell)
    {
        // Debug.Log($"over on cell {cell.X} : {cell.Y}");
    }

    public void MouseExit(Cell cell)
    {
        // Debug.Log($"exit on cell {cell.X} : {cell.Y}");
    }
}
