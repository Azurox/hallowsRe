using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightUIManager : MonoBehaviour {

    public GameObject ReadyButton;
    public GameObject FinishTurnButton;
    public TimelineUIComponent TimelineUIComponent;
    public StatsUIComponent StatsUIComponent;
    public SpellsUIComponent SpellsUIComponent;
    public ImpactUIComponent ImpactUIComponent;

    private MainFighterEmitter MainFighterEmitter;
    private FightMapHandler FightMapHandler;




    public void Init(MainFighterEmitter mainFighterEmitter, FightMapHandler fightMapHandler)
    {
        MainFighterEmitter = mainFighterEmitter;
        FightMapHandler = fightMapHandler;
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

    public void ShowSpells(List<Spell> spells)
    {
        SpellsUIComponent.SetSpells(spells);
    }


    public void UseSpell(Spell spell)
    {
        FightMapHandler.ShowSpellRange(spell);
    }

    public void ShowImpact(Impact impact, Vector2 position)
    {
        ImpactUIComponent.ShowImpact(impact, position);
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
