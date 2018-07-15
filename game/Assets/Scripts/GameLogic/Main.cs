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
        this.worldMapController = new WorldMapController(socket);
        this.fightMapController = new FightMapController(socket);

    }

}
