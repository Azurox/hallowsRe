using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDTG : MonoBehaviour {

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
            gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }else
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }

    private void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked on Fighter");
    }
}
