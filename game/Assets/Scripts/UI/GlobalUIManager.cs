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
        FightUIManager.gameObject.SetActive(true);
        WorldUIManager.gameObject.SetActive(false);
    }

    public void SwitchToWorldtUI()
    {
        WorldUIManager.gameObject.SetActive(true);
        FightUIManager.gameObject.SetActive(false);
    }
}
