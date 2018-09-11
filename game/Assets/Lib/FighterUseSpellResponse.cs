using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterUseSpellResponse {
    public string playerId;
    public Vector2 position;
    public string spellId;
    public ImpactResponse[] impacts;
    public FightEndResponse fightEnd;
}
