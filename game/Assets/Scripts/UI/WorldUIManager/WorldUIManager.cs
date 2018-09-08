using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIManager : MonoBehaviour {
    public AfterFightUIComponent AfterFightUIComponent;
    public ScenarioHolderUIComponent ScenarioHolderUIComponent;
    private MainPlayerHandler MainPlayerHandler;


    public void Init(MainPlayerHandler mainPlayerHandler)
    {
        MainPlayerHandler = mainPlayerHandler;
    }


    public void ShowAfterFightStats(AfterFightStats afterFightStats)
    {
        var go = Instantiate(AfterFightUIComponent, transform, false);
        go.GetComponent<AfterFightUIComponent>().ShowAfterfightStats(afterFightStats);
    }

    public void ShowScenario(Scenario scenario, NpcDTG npc)
    {
        ScenarioHolderUIComponent.gameObject.SetActive(true);
        ScenarioHolderUIComponent.InitScenario(scenario, npc);
    }

    public void SelectScenarioResponse(Scenario scenario, int responseIndex, Npc npc)
    {
        MainPlayerHandler.SelectScenarioResponse(scenario, responseIndex, npc);
    }

    public void HideScenario()
    {
        ScenarioHolderUIComponent.gameObject.SetActive(false);
    }
}
