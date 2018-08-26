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

    public void Move(List<Vector2> path)
    {
        Debug.Log("emit new path");

        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));

        JSONObject[] jsonPath = new JSONObject[path.Count];

        for (var i = 0; i < path.Count; i++)
        {

            Dictionary<string, JSONObject> pos = new Dictionary<string, JSONObject>();
            pos["x"] = new JSONObject((int)path[i].x);
            pos["y"] = new JSONObject((int)path[i].y);
            jsonPath[i] = new JSONObject(pos);

        }

        data["path"] = new JSONObject(jsonPath);

        socket.Emit("fighterMove", new JSONObject(data));
    }
}
