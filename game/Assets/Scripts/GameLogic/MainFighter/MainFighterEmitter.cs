using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MainFighterEmitter : MonoBehaviour
{

    private SocketManager socket;
    private string fightId;


    private void Start()
    {
        socket = FindObjectOfType<SocketManager>();
    }


    public void Init(string fightId)
    {
        this.fightId = fightId;
    }

    public void Teleport(Vector2 position)
    {
        /*Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
        data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
        data["x"] = new JSONObject(position.x);
        data["y"] = new JSONObject(position.y);
        socket.Emit("teleportPreFight", new JSONObject(data));*/
        TeleportRequest request = new TeleportRequest(fightId, position.x, position.y);
        socket.Emit("teleportPreFight", JsonConvert.SerializeObject(request));
    }

    public void Ready()
    {
        /*  Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
          data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
          socket.Emit("fighterReady", new JSONObject(data));*/
        socket.Emit("fighterReady", JsonConvert.SerializeObject(new FightIdRequest(fightId)));
    }

    public void FinishTurn()
    {
        /* Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
         data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
         socket.Emit("fighterFinishTurn", new JSONObject(data));*/
        socket.Emit("fighterFinishTurn", JsonConvert.SerializeObject(new FightIdRequest(fightId)));
    }

    public void Move(List<Vector2> path)
    {
        /*Debug.Log("emit new path");

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

        socket.Emit("fighterMove", new JSONObject(data));*/
    }

    public void UseSpell(Spell spell, Vector2 position)
    {
        /* Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();
         data["fightId"] = new JSONObject(string.Format("\"{0}\"", fightId));
         data["spellId"] = new JSONObject(string.Format("\"{0}\"", spell.id));

         Dictionary<string, JSONObject> pos = new Dictionary<string, JSONObject>();
         pos["x"] = new JSONObject((int)position.x);
         pos["y"] = new JSONObject((int)position.y);
         data["position"] = new JSONObject(pos);

         socket.Emit("fighterUseSpell", new JSONObject(data));*/
    }
}
