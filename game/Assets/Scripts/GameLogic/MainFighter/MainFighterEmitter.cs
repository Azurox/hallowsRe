using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFighterEmitter : MonoBehaviour {

    private SocketIOComponent socket;

    void Start()
    {
        socket = FindObjectOfType<SocketIOComponent>();
    }

    public void Teleport(Vector2 position, string fightId)
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
        data["x"] = new JSONObject(position.x);
        data["y"] = new JSONObject(position.y);
        socket.Emit("teleportPreFight", new JSONObject(data));
    }
}
