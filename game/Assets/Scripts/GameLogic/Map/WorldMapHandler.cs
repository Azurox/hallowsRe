using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapHandler : MonoBehaviour {
    private MainPlayerHandler mainPlayer;

    public void SetMainPlayer(MainPlayerHandler player)
    {
        mainPlayer = player;
    }

    public void TargetCell(int x, int y)
    {
        mainPlayer.TryMovement(x, y);
    }
}
