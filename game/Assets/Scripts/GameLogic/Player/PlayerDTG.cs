using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDTG : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on player");
        transform.parent.GetComponent<PlayerHandler>().ClickOnPlayer(this.name);
    }

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
    }
}
