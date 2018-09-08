using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterMiniatureUIComponent : MonoBehaviour {

    private FightUIManager FightUIManager;
    private FighterContainerDTG FighterContainerDTG;
    public Image Border;
    public RawImage Image;
    public Image Side;
    private Fighter fighter;

    public void Init(FightUIManager fightUIManager, FighterContainerDTG fighterContainerDTG)
    {
        FightUIManager = fightUIManager;
        FighterContainerDTG = fighterContainerDTG;
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
            Image.texture = PlayerInformation.Instance.GetFighterImage(true);
        }
        else
        {
            var fighterDtg = FighterContainerDTG.GetFighterDTGFromFighter(fighter);
            if(fighterDtg != null)
            {
                Image.texture = fighterDtg.GetImage();
            }
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
