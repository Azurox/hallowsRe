using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerEmitter : MonoBehaviour {

    private SocketIOComponent socket;

	void Start () {
        this.socket = FindObjectOfType<SocketIOComponent>();
	}

    public void NewPath(Vector2[] path)
    {

        JSONObject[] jsonPath = new JSONObject[path.Length];

        for(var i = 0; i < path.Length; i++)
        {

            Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
            data["x"] = new JSONObject((int)path[i].x);
            data["y"] = new JSONObject((int)path[i].y);
            jsonPath[i] = new JSONObject(data);

        }
        socket.Emit("initializeMovement", new JSONObject(jsonPath));
    }

    public void NewPosition(int x, int y)
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["x"] = new JSONObject(x);
        data["y"] = new JSONObject(y);
        socket.Emit("newPosition", new JSONObject(data));
    }

    public void StartFight(string id)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
        socket.Emit("startFight", new JSONObject(data));
    }
	
}
