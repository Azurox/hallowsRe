using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpellRequest
{
    public string fightId;
    public string spellId;
    public PositionRequest position;

    public UseSpellRequest(string fightId, string spellId, PositionRequest position)
    {
        this.fightId = fightId;
        this.spellId = spellId;
        this.position = position;
    }
}
