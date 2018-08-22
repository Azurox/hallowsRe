using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour {
    private MainPlayerHandler MainPlayer;

    public void SetMainPlayer(MainPlayerHandler player)
    {
        MainPlayer = player;
    }

    public void TargetCell(int x, int y)
    {
        MainPlayer.TryMovement(x, y);
    }
}
