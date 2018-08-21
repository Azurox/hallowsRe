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
            gameObject.GetComponent<Renderer>().material.color = new Color(16/255f, 39/255f, 191/255f);
        }else
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(244/255f, 65/255f, 98/255f);
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
