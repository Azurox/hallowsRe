using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterMiniatureUIComponent : MonoBehaviour {

    private FightUIManager FightUIManager;
    public Image Border;
    public Image Image;
    public Image Side;
    private Fighter fighter;

    public void Init(FightUIManager fightUIManager)
    {
        FightUIManager = fightUIManager;
    }

    public void MouseOverMiniature()
    {
        Debug.Log("show fighter statss");
        FightUIManager.ShowFighterStats(fighter);
    }

    public void SetFighter(Fighter fighter)
    {
        this.fighter = fighter;
        if (fighter.IsMainPlayer)
        {
            Image.color = new Color(1, 215 / 255f, 0 / 255f);
        }
        
        if(fighter.Side == global::Side.blue)
        {
            Side.color = new Color(0, 0, 1);
        }else
        {
            Side.color = new Color(1, 0, 0);
        }
    }

    public Fighter GetFighter()
    {
        return fighter;
    }

    public void HighlightBorder(bool highlight)
    {
        if (highlight)
        {
            Border.color = new Color(1, 1, 1);
        }else
        {
            Border.color = new Color(0, 0, 0, 0.4f);
        }
    }
}
