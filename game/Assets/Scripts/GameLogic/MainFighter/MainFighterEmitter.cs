using System;
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
        TeleportRequest request = new TeleportRequest(fightId, position.x, position.y);
        socket.Emit("teleportPreFight", JsonConvert.SerializeObject(request));
    }

    public void Ready()
    {
        socket.Emit("fighterReady", JsonConvert.SerializeObject(new FightIdRequest(fightId)));
    }

    public void FinishTurn()
    {
        socket.Emit("fighterFinishTurn", JsonConvert.SerializeObject(new FightIdRequest(fightId)));
    }

    public void Move(List<Vector2> path)
    {
        var pathRequests = new List<PositionRequest>();

        foreach (var position in path)
        {
            pathRequests.Add(new PositionRequest(position));
        }

        MoveRequest request = new MoveRequest(fightId, pathRequests);
        socket.Emit("fighterMove", JsonConvert.SerializeObject(request));
    }

    public void UseSpell(Spell spell, Vector2 position)
    {
        UseSpellRequest request = new UseSpellRequest(fightId, spell.id, new PositionRequest(position));
        socket.Emit("fighterUseSpell", JsonConvert.SerializeObject(request));
    }

    public void Checkin(string checkin)
    {
        CheckinRequest request = new CheckinRequest(fightId, checkin);
        Utils.Instance.DelayCoroutine(0.200f, () =>
        {
            socket.Emit("checkin", JsonConvert.SerializeObject(request));

        });
    }

}
