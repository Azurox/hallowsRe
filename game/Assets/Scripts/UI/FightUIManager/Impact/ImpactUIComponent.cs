using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactUIComponent : MonoBehaviour {

    public TextImpactUIComponent TextImpactUIComponent;
    private Camera Camera;

    private void Start()
    {
        Camera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }


    public void ShowImpact(Impact impact, Vector2 position)
    {
        Vector3 screenPos = Camera.WorldToScreenPoint(new Vector3(position.x, 0.5f, position.y));
        Debug.Log("  X" + screenPos.x + "  Y " + screenPos.y + "  Z" + screenPos.z);
        TextImpactUIComponent go;
        if(impact.life != 0)
        {
            go = Instantiate(TextImpactUIComponent);
            if(impact.life < 0)
            {
                go.SetText(TextImpactUIComponent.ImpactType.damage, impact.life);
            }
            else
            {
                go.SetText(TextImpactUIComponent.ImpactType.heal, impact.life);
            }
            go.transform.SetParent(transform);
            go.transform.position = screenPos;
        }

        Debug.Log("show impact of spell at " + position);
    }
}
