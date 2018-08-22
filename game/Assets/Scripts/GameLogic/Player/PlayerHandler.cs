using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    private MainPlayerHandler mainPlayer;

    public void SetMainPlayer(MainPlayerHandler player)
    {
        mainPlayer = player;
    }

    public void ClickOnPlayer(string id)
    {
        mainPlayer.StartFight(id);
    }
}
