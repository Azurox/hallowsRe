using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapController {

    private SocketIOComponent socket;

    public FightMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        InitSocket();
    }

    private void InitSocket()
    {
    }
}
