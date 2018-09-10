using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Main {

    private MapReceiver worldMapController;
    private FightMapReceiver fightMapController;
    private WebSocket socket;

    public Main()
    {
        worldMapController = new MapReceiver(socket);
        fightMapController = new FightMapReceiver(socket);
        //this.socket.Emit("initWorld");
      /*  socket = new WebSocket("ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket");

            socket.OnMessage += (sender, e) =>
                    Debug.Log("Laputa says: " + e.Data);

            socket.Connect();
            socket.Send("BALUS");*/

        
        
        Debug.Log("emit worlds");
    }

}
