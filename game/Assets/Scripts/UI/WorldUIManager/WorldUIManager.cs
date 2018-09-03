using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIManager : MonoBehaviour {
    public AfterFightUIComponent AfterFightUIComponent;


    public void ShowAfterFightStats(AfterFightStats afterFightStats)
    {
        var go = Instantiate(AfterFightUIComponent, transform, false);
        go.GetComponent<AfterFightUIComponent>().ShowAfterfightStats(afterFightStats);
    }

    public void ShowScenario(Scenario scenario)
    {

    }
}
