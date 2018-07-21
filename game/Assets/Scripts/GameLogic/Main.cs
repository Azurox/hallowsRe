using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main {

    private SocketIOComponent socket;
    private WorldMapController worldMapController;
    private FightMapController fightMapController;

    public Main(SocketIOComponent socket)
    {
        this.socket = socket;
        worldMapController = new WorldMapController(socket);
        fightMapController = new FightMapController(socket);
        this.socket.Emit("initWorld");
        //Debug.Log(this.socket);
        //Debug.Log("emit worlds");
    }

}
