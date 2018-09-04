using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioResponse {
    public string response;
    public string scenarioId;
    public bool end;

    public ScenarioResponse(string response, string scenarioId, bool end)
    {
        this.response = response;
        this.scenarioId = scenarioId;
        this.end = end;
    }
}
