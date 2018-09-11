using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRequest {
    public string fightId;
    public List<PositionRequest> path;

    public MoveRequest(string fightId, List<PositionRequest> path)
    {
        this.fightId = fightId;
        this.path = path;
    }
}
