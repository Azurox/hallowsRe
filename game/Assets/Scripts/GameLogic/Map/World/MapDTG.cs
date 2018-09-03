using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDTG : MonoBehaviour {

    private CellDTG[,] cells;
    private GameMap currentMap;

    public void Init()
    {
        currentMap = GetComponent<GlobalMapDTG>().GetMap();

        var oldCells = GetComponent<GlobalMapDTG>().GetCells();

        cells = new CellDTG[oldCells.GetLength(0), oldCells.GetLength(1)];

        for (var i = 0; i < oldCells.GetLength(0); i++)
        {
            for (var j = 0; j < oldCells.GetLength(1); j++)
            {
                if (oldCells[i, j] == null) continue;
                cells[i, j] = oldCells[i, j].GetComponent<CellDTG>();
            }
        }
    }

    public GameMap GetMap()
    {
        return currentMap;
    }

    public CellDTG[,] GetCells()
    {
        return cells;
    }

}
