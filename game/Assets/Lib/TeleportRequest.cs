using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRequest {

    public string fightId;
    public int y;
    public int x;

    public TeleportRequest(string fightId, float x, float y)
    {
        this.fightId = fightId;
        this.x = (int) x;
        this.y = (int) y;
    }
}
