using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact {

    public string playerId;
    public int life;

    public Impact(JSONObject data)
    {
        playerId = data["playerId"] != null ? data["playerId"].str : null;
        life = data["life"] != null ? (int) data["life"].n : 0;
    }
}
