using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour {
    private WorldMapHandler WorldMapHandler;
    private FightMapHandler FightMapHandler;
    private bool isFighting = false;

    private void Start()
    {
        WorldMapHandler = GetComponent<WorldMapHandler>();
        FightMapHandler = GetComponent<FightMapHandler>();
    }

    public void SetMainPlayer(MainPlayerHandler player)
    {
        GetComponent<WorldMapHandler>().SetMainPlayer(player);
    }

    public void SetIsFighting(bool boolean)
    {
        isFighting = true;
    }
	
    public void TargetCell(int x, int y)
    {
        if (isFighting)
        {
            FightMapHandler.TargetCell(x, y);
        } else
        {
            WorldMapHandler.TargetCell(x, y);
        }
    }
}
