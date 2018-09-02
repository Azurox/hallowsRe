using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterFightStats {

    public bool win;
    public int xp;
    public int gold;

	public AfterFightStats(JSONObject data)
    {
        win = data["win"] != null ? data["win"].b : false;
        xp = data["xp"] != null ? (int) data["xp"].n : 0;
        gold = data["gold"] != null ? (int)data["gold"].n : 0;
        // Need to add looted items.
    }
}
