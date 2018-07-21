using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDTG : MonoBehaviour {

    public GameObject CellGameObject;
    private CellDTG[,] cells;
    private GameMap currentMap;

    public void SetMap(GameMap map)
    {
        currentMap = map;
        ReloadMap();
    }

    private void ReloadMap()
    {
        for (int i = 0, length = currentMap.cells.GetLength(0); i < length; i++)
        {
            for (int j = 0, lengthJ = currentMap.cells.GetLength(1); j < lengthJ; i++)
            {
                cells[i,j] = Instantiate(CellGameObject).GetComponent<CellDTG>();
                cells[i,j].SetCell(currentMap.cells[i, j]);
            }
        }
    }

}
