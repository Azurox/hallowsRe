using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainFighterHandler : MonoBehaviour {

    private FightMapHandler fightMapHandler;
    private MainFighterDTG mainFighterDTG;

    public void Startup(FightMapHandler fightMapHandler)
    {
        this.fightMapHandler = fightMapHandler;
    }

    public void Init(MainFighterDTG mainFighterDTG)
    {
        this.mainFighterDTG = mainFighterDTG;
    }

    public void ClickOnMainFighter(Fighter fighter)
    {
        fightMapHandler.TargetCell((int)fighter.Position.x, (int)fighter.Position.y);
    }

    public void MouseOverMainFighter(Fighter fighter)
    {
        fightMapHandler.MouseOverCell((int)fighter.Position.x, (int)fighter.Position.y);
        fightMapHandler.ShowMovementRange(fighter.Position, fighter.CurrentMovementPoint);
        GetComponent<FighterContainerDTG>().FocusFighter(fighter);
    }

    public void MouseExitMainFighter(Fighter fighter)
    {
        fightMapHandler.MouseLeftCell((int)fighter.Position.x, (int)fighter.Position.y);
        fightMapHandler.HideMovementRange();
    }

    public bool IsMovementPossible(Vector2 position)
    {
        var fighterPosition = mainFighterDTG.GetFighter().Position;
        var movementPoint = mainFighterDTG.GetFighter().CurrentMovementPoint;

        var totalVector = position - fighterPosition;
        var distance = System.Math.Abs(totalVector.x) + System.Math.Abs(totalVector.y);

        if(distance > movementPoint)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
