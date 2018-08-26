﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainFighterHandler : MonoBehaviour {

    private FightMapHandler fightMapHandler;
    private List<Vector2> possibleMovementPosition;
    private bool possibleMovementPositionDirty = false;
    private MainFighterDTG mainFighterDTG;

    public void Startup(FightMapHandler fightMapHandler)
    {
        this.fightMapHandler = fightMapHandler;
    }

    public void Init(MainFighterDTG mainFighterDTG)
    {
        this.mainFighterDTG = mainFighterDTG;
    }

    public void MouseOverMainFighter(Fighter fighter)
    {
        fightMapHandler.ShowMovementRange(fighter.Position, fighter.CurrentMovementPoint);
        GetComponent<FighterContainerDTG>().FocusFighter(fighter);
    }

    public void MouseExitMainFighter()
    {
        fightMapHandler.HideMovementRange();
    }

    public bool IsMovementPossible(Vector2 position)
    {
        if(possibleMovementPosition == null || possibleMovementPositionDirty)
        {
            possibleMovementPosition = fightMapHandler.GetMovementRange(mainFighterDTG.GetFighter().Position, mainFighterDTG.GetFighter().CurrentMovementPoint);
            possibleMovementPositionDirty = false;
        }

        if(!possibleMovementPosition.Any(i => i == position))
        {
            return false;
        }
        else{
            return true;
        }

    }

    public void MainFighterMoved()
    {
        possibleMovementPositionDirty = true;
        possibleMovementPosition = null;
    }
}
