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
        fightMapHandler.TargetCell((int)fighter.Position.x, (int)fighter.Position.y);
    }

    public void MouseOverFighter(Fighter fighter)
    {
        fightMapHandler.MouseOverCell((int)fighter.Position.x, (int)fighter.Position.y);
        fightMapHandler.ShowMovementRange(fighter.Position, fighter.CurrentMovementPoint);
        GetComponent<FighterContainerDTG>().FocusFighter(fighter);
    }

    public void MouseExitFighter(Fighter fighter)
    {
        fightMapHandler.MouseLeftCell((int)fighter.Position.x, (int)fighter.Position.y);
        fightMapHandler.HideMovementRange();
    }
}
