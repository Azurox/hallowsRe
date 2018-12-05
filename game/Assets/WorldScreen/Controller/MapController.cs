using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    private SocketManager socket;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void LoapMap(string data)
    {

    }
}
