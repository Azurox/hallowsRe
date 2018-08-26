using System.Collections.Generic;
using UnityEngine;

public class FightMapHandler : MonoBehaviour {

    private FightMapDTG fightMapDTG;
    private FighterHandler fighterHandler;
    private MainFighterHandler mainFighterHandler;
    private MainFighterEmitter mainFighterEmitter;
    private MainFighterDTG mainFighter;
    private Fight fight;

    public void Startup(FighterContainerDTG fighterContainerDTG)
    {
        fightMapDTG = GetComponent<FightMapDTG>();
        fighterHandler = fighterContainerDTG.GetComponent<FighterHandler>();
        mainFighterHandler = fighterContainerDTG.GetComponent<MainFighterHandler>();
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
        }
    }

    public void MouseOverCell(int x, int y)
    {
        if(fight.phase == 1)
        {
            FindPath(new Vector2(x, y));
        }
    }

    public void MouseLeftCell(int x, int y)
    {
        fightMapDTG.ResetPathCells();
    }

    public void ShowMovementRange(Vector2 position, int range)
    {
        var cells = GetComponent<FightMapPathFinding>().FindRange(position, range);
        foreach (var cell in cells)
        {
            fightMapDTG.SetCellMovementColor(new Vector2(cell.currentCell.X, cell.currentCell.Y));
        }
    }

    public List<Vector2> GetMovementRange(Vector2 position, int range)
    {
        var cells = GetComponent<FightMapPathFinding>().FindRange(position, range);
        List<Vector2> positions = new List<Vector2>();
        foreach (var cell in cells)
        {
            positions.Add(new Vector2(cell.currentCell.X, cell.currentCell.Y));
        }
        return positions;
    }

    public void HideMovementRange()
    {
        fightMapDTG.ResetMovementCells();
    }

    public void FindPath(Vector2 position)
    {
        if (mainFighterHandler.isMovementPossible(position))
        {
           var path = GetComponent<FightMapPathFinding>().FindPath(mainFighter.GetFighter().Position, position);
           fightMapDTG.HighlightPath(path);
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
