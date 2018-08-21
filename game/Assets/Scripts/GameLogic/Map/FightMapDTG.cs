using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapDTG : MonoBehaviour {

    private FightCellDTG[,] cells;


    public void Init()
    {
        CellDTG[,] oldCells = GetComponent<MapDTG>().GetCells();

        if (cells == null)
        {
            cells = new FightCellDTG[oldCells.GetLength(0), oldCells.GetLength(1)];
        }

        for(var i = 0; i < oldCells.GetLength(0); i++)
        {
            for (var j = 0; j < oldCells.GetLength(1); j++)
            {
                oldCells[i, j].GetComponent<FightCellDTG>().enabled = true;
                cells[i, j] = oldCells[i, j].GetComponent<FightCellDTG>();
                oldCells[i, j].enabled = false;
            }
        }
    }

    public void SetSpawnCell(Side side, Vector2 position, bool taken)
    {
        SetCellAvailability(position, taken);
        if(side == Side.blue)
        {
            cells[(int)position.x, (int)position.y].GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }else
        {
            cells[(int)position.x, (int)position.y].GetComponent<Renderer>().material.color = new Color(255, 0, 0);

        }

    }

    public void SetCellAvailability(Vector2 position, bool taken)
    {
        cells[(int)position.x, (int)position.y].taken = taken;
    }
}
