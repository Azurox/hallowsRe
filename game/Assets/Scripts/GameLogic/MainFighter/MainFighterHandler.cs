using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFighterHandler : MonoBehaviour {

    private FightMapHandler fightMapHandler;

    public void Startup(FightMapHandler fightMapHandler)
    {
        this.fightMapHandler = fightMapHandler;
    }

    public void MouseOverMainFighter(Fighter fighter)
    {
        fightMapHandler.ShowMovementRange(fighter.Position, fighter.MovementPoint);
        GetComponent<FighterContainerDTG>().FocusFighter(fighter);
    }

    public void MouseExitMainFighter()
    {
        fightMapHandler.HideMovementRange();
    }
}
