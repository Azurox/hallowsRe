using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour {
    private WorldMapHandler WorldMapHandler;
    private FightMapHandler FightMapHandler;

    private void Start()
    {
        WorldMapHandler = GetComponent<WorldMapHandler>();
        FightMapHandler = GetComponent<FightMapHandler>();
    }

    public void SetMainPlayer(MainPlayerHandler player)
    {
        GetComponent<WorldMapHandler>().SetMainPlayer(player);
    }
}
