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
        ActivateReadyButton(true);
        ActivateFinishTurnButton(false);
    }

    public void SetUIPhase1()
    {
        ActivateReadyButton(false);
        ActivateFinishTurnButton(true);
    }

    public void ActivateReadyButton(bool activate)
    {
        ReadyButton.SetActive(activate);
    }

    public void ActivateFinishTurnButton(bool activate)
    {
        FinishTurnButton.SetActive(activate);

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
        var isMainPlayer = TimelineUIComponent.HighlightFighter(id);
        if (isMainPlayer)
        {
            ActivateFinishTurnButton(true);
        }
        else
        {
            ActivateFinishTurnButton(false);
        }
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
