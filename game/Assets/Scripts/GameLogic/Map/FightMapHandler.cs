using UnityEngine;

public class FightMapHandler : MonoBehaviour {

    private FightMapDTG fightMapDTG;
    private FighterHandler fighterHandler;
    private MainFighterEmitter mainFighterEmitter;
    private MainFighterDTG mainFighter;
    private Fight fight;

    public void Startup(FightMapDTG fightMapDTG)
    {
        this.fightMapDTG = fightMapDTG;
        fighterHandler = fightMapDTG.GetComponent<FighterHandler>();
    }

    public void Init(MainFighterDTG mainFighter, Fight fight)
    {
        this.fight = fight;
        this.mainFighter = mainFighter;
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

    public void ShowMovementRange(Vector2 position, int range)
    {
        var cells = fightMapDTG.FindRange(position, range);
        foreach (var cell in cells)
        {
            fightMapDTG.SetCellMovementColor(new Vector2(cell.currentCell.X, cell.currentCell.Y));
        }
    }

    public void HideMovementRange()
    {
        fightMapDTG.ResetMovementCell();
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
