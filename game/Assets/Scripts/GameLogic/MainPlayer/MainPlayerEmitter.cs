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
        socket.Emit("newPosition", JsonConvert.SerializeObject(new PositionRequest(x, y)));
    }

    public void StartFight(string id)
    {
        socket.Emit("startFight", JsonConvert.SerializeObject(new StartFightRequest(id)));
    }

    public void SelectScenarioResponse(Scenario scenario, int responseIndex, Npc npc)
    {
        var request =  new SelectScenarioRequest(scenario.id, responseIndex, npc.id);
        socket.Emit("finishScenario", JsonConvert.SerializeObject(request));
    }

    public void StartMonsterFight(string id)
    {
        Debug.Log("Start fight with monster group : " + id);
        socket.Emit("startMonsterFight", JsonConvert.SerializeObject(new StartFightRequest(id)));
    }
}
