using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFighterEmitter : MonoBehaviour {

    private SocketIOComponent socket;
    private string fightId;


    void Start()
    {
        socket = FindObjectOfType<SocketIOComponent>();
    }

    public void Init(string fightId)
    {
        this.fightId = fightId;
    }

    public void Teleport(Vector2 position)
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
        data["x"] = new JSONObject(position.x);
        data["y"] = new JSONObject(position.y);
        socket.Emit("teleportPreFight", new JSONObject(data));
    }

    public void Ready()
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
        socket.Emit("fighterReady", new JSONObject(data));
    }

    public void FinishTurn()
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
        socket.Emit("fighterFinishTurn", new JSONObject(data));
    }
}
