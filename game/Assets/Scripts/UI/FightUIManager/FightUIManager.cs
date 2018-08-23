using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightUIManager : MonoBehaviour {

    public GameObject ReadyButton;
    public TimelineUIComponent TimelineUIComponent;
    public StatsUIComponent StatsUIComponent;



    public void Init()
    {

    }

    public void SetUIPhase0()
    {
        ReadyButton.SetActive(true);
    }

    public void SetUIPhase1()
    {
        ReadyButton.SetActive(false);
    }


    public void ShowFighterStats(Fighter fighter)
    {
        StatsUIComponent.SetFighter(fighter);
    }

    public void UpdateFightTimeline(List<Fighter> fighters)
    {
        TimelineUIComponent.UpdateFightTimeline(fighters);
    }


    /* UI Action */

    public void FinishTurn()
    {
        Debug.Log("finish turn");
    }

	public void SetReady()
    {
        Debug.Log("ready");
    }

}
