using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketManager : MonoBehaviour
{

    WebSocket socket;
    // Use this for initialization
    IEnumerator Start()
    {
        // socket = new WebSocket("ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket");
        // socket.OnMessage += (sender, e) =>
        // Debug.Log("Laputa says: " + e.Data);
        // socket.Connect();

        using (socket = new WebSocket("ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket"))
        {
            socket.OnMessage += (sender, e) =>
                Debug.Log("Laputa says: " + e.Data);
                socket.Connect();

            while (true)
            {
                socket.Ping();
                socket.Send("2");
                yield return new WaitForSeconds(1);

            }
        }
    }

}
