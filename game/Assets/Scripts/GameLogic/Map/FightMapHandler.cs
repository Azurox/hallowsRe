using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapHandler : MonoBehaviour {

    private FightMapDTG fightMapDTG;
    private FighterHandler fighterHandler;
    private MainFighterEmitter mainFighterEmitter;
    private Fight fight;
    private Fighter mainPlayer;

    public void Init(FightMapDTG fightMapDTG, Fight fight)
    {
        this.fightMapDTG = fightMapDTG;
        this.fight = fight;
        fighterHandler = fightMapDTG.GetComponent<FighterHandler>();
        mainPlayer = fight.GetMainPlayer();
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
        if(mainPlayer.Side == Side.blue)
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
