using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMiniatureUIComponent : MonoBehaviour {

    private FightUIManager FightUIManager;
    private Spell spell;


    public void Init(FightUIManager fightUIManager)
    {
        FightUIManager = fightUIManager;
    }


    public void ClickOnMiniature()
    {
        FightUIManager.UseSpell(spell);
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
    }
}
