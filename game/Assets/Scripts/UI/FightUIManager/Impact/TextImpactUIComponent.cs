using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImpactUIComponent : MonoBehaviour {

    public Text text;

    void Start()
    {
         Destroy (gameObject, GetComponentInChildren<Animation>().clip.length);     
    }

    public enum ImpactType
    {
        damage,
        heal,
        actionPoint,
        movementPoint
    }

    public void SetText(ImpactType impactType, int num)
    {
        switch (impactType)
        {
            case ImpactType.damage:
                text.color = new Color(1, 0, 0);
                text.text = num.ToString();
                break;
            case ImpactType.heal:
                text.color = new Color(0, 1, 0);
                text.text = "+" + num.ToString();
                break;
        }
    }

}
