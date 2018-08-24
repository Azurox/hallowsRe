using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightUIManager : MonoBehaviour {

    public GameObject ReadyButton;
    public GameObject FinishTurnButton;
    public TimelineUIComponent TimelineUIComponent;
    public StatsUIComponent StatsUIComponent;
    private MainFighterEmitter MainFighterEmitter;



    public void Init(MainFighterEmitter mainFighterEmitter)
    {
        MainFighterEmitter = mainFighterEmitter;
    }

    public void SetUIPhase0()
    {
        ReadyButton.SetActive(true);
        FinishTurnButton.SetActive(false);
    }

    public void SetUIPhase1()
    {
        ReadyButton.SetActive(false);
        FinishTurnButton.SetActive(true);
    }


    public void ShowFighterStats(Fighter fighter)
    {
        StatsUIComponent.SetFighter(fighter);
    }

    public void UpdateFightTimeline(List<Fighter> fighters)
    {
        TimelineUIComponent.UpdateFightTimeline(fighters);
    }

    public void HighlightFighter(string id)
    {
        TimelineUIComponent.HighlightFighter(id);
    }


    /* UI Action */

    public void FinishTurn()
    {
        MainFighterEmitter.FinishTurn();
    }

	public void SetReady()
    {
        MainFighterEmitter.Ready();
    }

}
