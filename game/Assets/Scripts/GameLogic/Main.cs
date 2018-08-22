using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main {

    private SocketIOComponent socket;
    private MapReceiver worldMapController;
    private FightMapReceiver fightMapController;

    public Main(SocketIOComponent socket)
    {
        this.socket = socket;
        worldMapController = new MapReceiver(socket);
        fightMapController = new FightMapReceiver(socket);
        this.socket.Emit("initWorld");
        Debug.Log("emit worlds");
    }

}
