﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFighterDTG : MonoBehaviour {

    private Fighter fighter;

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

    private void OnMouseDown()
    {
        Debug.Log("Clicked on self");
    }

    private void OnMouseEnter()
    {
        transform.parent.GetComponent<FighterHandler>().MouseOverFighter(fighter);
    }
}
