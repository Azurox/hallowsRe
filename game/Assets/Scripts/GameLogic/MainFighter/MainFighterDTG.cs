using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainFighterDTG : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Fighter fighter;
    private bool isHoveringBlocked = false;

    public void SetFighter(Fighter fighter)
    {
        this.fighter = fighter;
    }

    public Fighter GetFighter()
    {
        return fighter;
    }

    public void InitFighter()
    {
        SetPosition((int)fighter.Position.x, (int)fighter.Position.y);
        gameObject.GetComponent<Renderer>().material.color = new Color(255 / 255f, 215 / 255f, 0 / 255f);
    }

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
        fighter.Position = new Vector2(x, y);
    }

    public void TakeImpact(Impact impact, Action callback)
    {
        fighter.TakeImpact(impact);
        //play an animation
        if (callback != null)
        {
            callback();
        }
    }

    public void UseSpell(Spell spell, Vector2 position, System.Action callback)
    {
        fighter.UpdateCurrentActionPoint(-spell.actionPointCost);
        //play an animation
        if (callback != null)
        {
            callback();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.GetComponent<MainFighterHandler>().ClickOnMainFighter(fighter);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHoveringBlocked)
        {
            transform.parent.GetComponent<MainFighterHandler>().MouseOverMainFighter(fighter);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<MainFighterHandler>().MouseExitMainFighter(fighter);
    }

    public void BlockHovering(bool blockIt)
    {
        isHoveringBlocked = blockIt;
    }

}
