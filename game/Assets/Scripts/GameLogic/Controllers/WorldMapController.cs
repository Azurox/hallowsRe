using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapController {

    private SocketIOComponent socket;

    public WorldMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("loadMap", (data) =>
        {
            Debug.Log(data);
        });
    }
}
