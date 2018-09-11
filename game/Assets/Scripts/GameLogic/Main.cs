using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Main {

    private MapReceiver worldMapController;
    private FightMapReceiver fightMapController;

    public Main(SocketManager socket)
    {
        worldMapController = new MapReceiver(socket);
        fightMapController = new FightMapReceiver(socket);
        socket.Emit("initWorld");        
    }

}
