using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterHandler : MonoBehaviour {
    private FightMapHandler fightMapHandler;
    private MainFighterHandler mainFighter;

    public void Startup(FightMapHandler fightMapHandler)
    {
        this.fightMapHandler = fightMapHandler;
    }

    public void SetMainFighter(MainFighterDTG fighter)
    {
        mainFighter = fighter.GetComponent<MainFighterHandler>();
    }

    public void ClickOnFighter(Fighter fighter)
    {

    }

    public void MouseOverFighter(Fighter fighter)
    {
        fightMapHandler.ShowMovementRange(fighter.Position, fighter.MovementPoint);
        GetComponent<FighterContainerDTG>().FocusFighter(fighter);
    }

    public void MouseExitFighter()
    {
        fightMapHandler.HideMovementRange();
    }
}
