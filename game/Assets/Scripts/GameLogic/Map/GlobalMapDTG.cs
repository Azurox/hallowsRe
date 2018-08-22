using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapDTG : MonoBehaviour
{
    public GameObject CellGameObject;
    private GameObject[,] cells;
    private GameMap currentMap;

    public void SetMap(GameMap map)
    {
        currentMap = map;
        if (cells == null)
        {
            cells = new GameObject[map.cells.GetLength(0), map.cells.GetLength(1)];
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
                cells[j, i] = cell;
                cells[j, i].GetComponent<CellDTG>().SetCell(currentMap.cells[i, j]);
                cells[j, i].GetComponent<FightCellDTG>().SetCell(currentMap.cells[i, j]);
            }
        }

    }

    public GameObject[,] GetCells()
    {
        return cells;
    }

    public void ActivateFightCell()
    {

        for (var i = 0; i < cells.GetLength(0); i++)
        {
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j].GetComponent<FightCellDTG>().enabled = true;
                cells[i, j].GetComponent<CellDTG>().enabled = false;

            }
        }

    }

    public void ActivateCell()
    {

        for (var i = 0; i < cells.GetLength(0); i++)
        {
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j].GetComponent<FightCellDTG>().enabled = false;
                cells[i, j].GetComponent<CellDTG>().enabled = true;

            }
        }

    }

}
