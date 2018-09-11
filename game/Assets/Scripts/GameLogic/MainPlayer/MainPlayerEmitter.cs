using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MainPlayerEmitter : MonoBehaviour {

    private SocketManager socket;

    private void Start()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void NewPath(Vector2[] path)
    {
        var pathRequests = new List<PositionRequest>();

        foreach (var position in path)
        {
            pathRequests.Add(new PositionRequest(position));
        }

        /*JSONObject[] jsonPath = new JSONObject[path.Length];

        for(var i = 0; i < path.Length; i++)
        {

            Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
            data["x"] = new JSONObject((int)path[i].x);
            data["y"] = new JSONObject((int)path[i].y);
            jsonPath[i] = new JSONObject(data);

        }*/
        socket.Emit("initializeMovement", JsonConvert.SerializeObject(pathRequests));
    }

    public void NewPosition(int x, int y)
    {
        /*Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["x"] = new JSONObject(x);
        data["y"] = new JSONObject(y);
        socket.Emit("newPosition", new JSONObject(data));*/
    }

    public void StartFight(string id)
    {
        /*Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
        socket.Emit("startFight", new JSONObject(data));*/
    }

    public void SelectScenarioResponse(Scenario scenario, int responseIndex, Npc npc)
    {
        /*Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["scenarioId"] = new JSONObject(string.Format("\"{0}\"", scenario.id));
        data["responseIndex"] = new JSONObject(responseIndex);
        data["npcId"] = new JSONObject(string.Format("\"{0}\"", npc.id));
        socket.Emit("finishScenario", new JSONObject(data));*/
    }

}
