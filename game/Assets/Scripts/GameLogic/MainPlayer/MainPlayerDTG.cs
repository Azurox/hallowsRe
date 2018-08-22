using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerDTG : MonoBehaviour {

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked on self");
    }
}
