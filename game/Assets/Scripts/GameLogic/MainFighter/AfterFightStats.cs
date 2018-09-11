using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterFightStats {

    public bool win;
    public int xp;
    public int gold;

	public AfterFightStats(FightEndResponse data)
    {
        win = data.win;
        xp = data.xp;
        gold = data.gold;
        // Need to add looted items.
    }
}
