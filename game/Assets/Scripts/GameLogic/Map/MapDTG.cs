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
        if(cells == null)
        {
            cells = new CellDTG[map.cells.GetLength(0), map.cells.GetLength(1)];
        }
        ReloadMap();
    }
    
    public GameMap GetMap()
    {
        return currentMap;
    }

    private void ReloadMap()
    {
        for (int i = 0, length = currentMap.cells.GetLength(0); i < length; i++)
        {
            for (int j = 0, lengthJ = currentMap.cells.GetLength(1); j < lengthJ; j++)
            {
                GameObject cell = Instantiate(CellGameObject);
                cell.transform.parent = gameObject.transform;
                cell.name = j + "-" + i;
                cells[i, j] = cell.GetComponent<CellDTG>() ;
                cells[i,j].SetCell(currentMap.cells[i, j]);

            }
        }
    }

}
