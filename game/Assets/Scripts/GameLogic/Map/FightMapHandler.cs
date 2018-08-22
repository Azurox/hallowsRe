using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapHandler : MonoBehaviour {

    private FightMapDTG fightMapDTG;
    private FighterHandler fighterHandler;
    private MainFighterEmitter mainFighterEmitter;
    private MainFighterDTG mainFighter;
    private Fight fight;

    public void Init(MainFighterDTG mainFighter, FightMapDTG fightMapDTG, Fight fight)
    {
        this.fightMapDTG = fightMapDTG;
        this.fight = fight;
        this.mainFighter = mainFighter;
        fighterHandler = fightMapDTG.GetComponent<FighterHandler>();
        mainFighterEmitter = mainFighter.GetComponent<MainFighterEmitter>();
    }

    public void TargetCell(int x, int y)
    {
        if(fight.phase == 0)
        {
            TryTeleport(x, y);
        } else
        {
           Debug.Log("why you do dis to me");
        }
    }

    private void TryTeleport(int x, int y)
    {
        if(mainFighter.GetFighter().Side == Side.blue)
        {
            foreach (var cell in fightMapDTG.blueCells)
            {
                if(cell.currentCell.X == x && cell.currentCell.Y == y)
                {
                    mainFighterEmitter.Teleport(new Vector2(x, y));
                    Debug.Log("Movement is legal blue side");
                }
            }
        } else
        {
            foreach (var cell in fightMapDTG.redCells)
            {
                if (cell.currentCell.X == x && cell.currentCell.Y == y)
                {
                    mainFighterEmitter.Teleport(new Vector2(x, y));
                    Debug.Log("Movement is legal red side");
                }
            }
        }
    }
}
