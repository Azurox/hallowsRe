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

        socket.Emit("initializeMovement", JsonConvert.SerializeObject(pathRequests));
    }

    public void NewPosition(int x, int y)
    {
        /*Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["x"] = new JSONObject(x);
        data["y"] = new JSONObject(y);
        socket.Emit("newPosition", new JSONObject(data));*/
        socket.Emit("initializeMovement", JsonConvert.SerializeObject(new PositionRequest(x, y)));

    }

    public void StartFight(string id)
    {
        /*Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
        socket.Emit("startFight", new JSONObject(data));*/
        socket.Emit("startFight", JsonConvert.SerializeObject(new StartFightRequest(id)));
    }

    public void SelectScenarioResponse(Scenario scenario, int responseIndex, Npc npc)
    {
        /*Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["scenarioId"] = new JSONObject(string.Format("\"{0}\"", scenario.id));
        data["responseIndex"] = new JSONObject(responseIndex);
        data["npcId"] = new JSONObject(string.Format("\"{0}\"", npc.id));
        socket.Emit("finishScenario", new JSONObject(data));*/
        var request =  new SelectScenarioRequest(scenario.id, responseIndex, npc.id);
        socket.Emit("finishScenario", JsonConvert.SerializeObject(request));
    }

}
