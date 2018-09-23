using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckinRequest
{
    public string fightId;
    public string checkId;

    public CheckinRequest(string fightId, string checkId)
    {
        this.fightId = fightId;
        this.checkId = checkId;
    }
}
