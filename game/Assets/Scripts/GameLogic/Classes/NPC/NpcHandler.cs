using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHandler : MonoBehaviour {

    private WorldUIManager WorldUIManager;

    public void Startup(WorldUIManager worldUIManager)
    {
        WorldUIManager = worldUIManager;
    }

    public void ShowScenario(Scenario scenario, NpcDTG npc)
    {
        WorldUIManager.ShowScenario(scenario, npc);
    }
}
