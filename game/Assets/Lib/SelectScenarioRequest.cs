using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScenarioRequest
{
    public string scenarioId;
    public int responseIndex;
    public string npcId;

    public SelectScenarioRequest(string scenarioId, int responseIndex, string npcId)
    {
        this.scenarioId = scenarioId;
        this.responseIndex = responseIndex;
        this.npcId = npcId;
    }
}
