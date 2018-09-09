using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPlayerDTG : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on self");
    }

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
    }
}
