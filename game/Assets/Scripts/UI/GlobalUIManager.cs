using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour {

    private FightUIManager FightUIManager;
    private WorldUIManager WorldUIManager;

    void Start () {
        FightUIManager = GetComponentInChildren<FightUIManager>();
        WorldUIManager = GetComponentInChildren<WorldUIManager>();
    }
	
	public void SwitchToFightUI()
    {
        WorldUIManager.GetComponent<Canvas>().enabled = false;
        FightUIManager.GetComponent<Canvas>().enabled = true;
    }

    public void SwitchToWorldUI()
    {
        WorldUIManager.GetComponent<Canvas>().enabled = true;
        FightUIManager.GetComponent<Canvas>().enabled = false;
    }

    public FightUIManager GetFightUIManager()
    {
        return FightUIManager;
    }

    public WorldUIManager GetWorldUIManager()
    {
        return WorldUIManager;
    }
}
