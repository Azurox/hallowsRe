using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FighterDTG : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Fighter fighter;

    public void SetFighter(Fighter fighter)
    {
        this.fighter = fighter;
    }

    public void InitFighter()
    {
        SetPosition((int)fighter.Position.x, (int)fighter.Position.y);
        if(fighter.Side == Side.blue)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(16/255f, 39/255f, 191/255f);
        }else
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(244/255f, 65/255f, 98/255f);
        }
    }

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
        fighter.Position = new Vector2(x, y);
    }

    public Fighter GetFighter()
    {
        return fighter;
    }

    public void TakeImpact(Impact impact, Action callback)
    {
        fighter.TakeImpact(impact);
        //play an animation
        if (callback != null) {
            callback();
        }
    }

    public void UseSpell(Spell spell, Vector2 position, System.Action callback )
    {
        fighter.UpdateCurrentActionPoint(-spell.actionPointCost);
        //play an animation
        if(callback != null)
        {
            callback();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<FighterHandler>().ClickOnFighter(fighter);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.GetComponent<FighterHandler>().MouseOverFighter(fighter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.GetComponent<FighterHandler>().MouseExitFighter(fighter);
    }
}
