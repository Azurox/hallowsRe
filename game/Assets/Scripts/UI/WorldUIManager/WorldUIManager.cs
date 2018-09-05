﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIManager : MonoBehaviour {
    public AfterFightUIComponent AfterFightUIComponent;
    public ScenarioHolderUIComponent ScenarioHolderUIComponent;


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

    public void HideScenario()
    {
        ScenarioHolderUIComponent.gameObject.SetActive(false);
    }
}
