using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHandler : MonoBehaviour {

    private WorldUIManager WorldUIManager;

    public void Init(WorldUIManager worldUIManager)
    {
        WorldUIManager = worldUIManager;
    }

    public void ShowScenario(Scenario scenario)
    {
        WorldUIManager.ShowScenario(scenario);
    }
}
