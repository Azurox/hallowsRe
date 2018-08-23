using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour {

    public FightUIManager FightUIManager;
    public WorldUIManager WorldUIManager;

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
}
