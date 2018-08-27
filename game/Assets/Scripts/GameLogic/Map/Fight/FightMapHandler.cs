using System.Collections.Generic;
using UnityEngine;

public class FightMapHandler : MonoBehaviour {

    private FightMapDTG fightMapDTG;
    private FighterHandler fighterHandler;
    private MainFighterHandler mainFighterHandler;
    private MainFighterEmitter mainFighterEmitter;
    private MainFighterDTG mainFighter;
    private Fight fight;
    private Spell selectedSpell;


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
        else
        {
            if (fight.IsMainFighterTurn())
            {

                if(selectedSpell != null)
                {
                    //mainFighterEmitter.UseSpell(selectedSpell)
                }
                else
                {
                    if (fightMapDTG.GetHighlightedPath().Count != 0)
                    {
                        mainFighterEmitter.Move(fightMapDTG.GetHighlightedPath());
                    }
                }
            }
        }
    }

    public void MouseOverCell(int x, int y)
    {
        if(fight.phase == 1 && fight.IsMainFighterTurn())
        {
            if(selectedSpell != null && fightMapDTG.SpellImpactIsInRange(new Vector2(x, y)))
            {
                fightMapDTG.HighlightSpellImpact(selectedSpell.hitArea, new Vector2(x, y));
            }
            else
            {
                FindPath(new Vector2(x, y));
            }
        }
    }

    public void MouseLeftCell(int x, int y)
    {
        if(selectedSpell != null)
        {
            fightMapDTG.ResetSpellImpact();
        }else
        {
            fightMapDTG.ResetPathCells();
        }
    }

    public void ShowMovementRange(Vector2 position, int range)
    {
        /*
        var cells = GetComponent<FightMapPathFinding>().FindRange(position, range);
        foreach (var cell in cells)
        {
            fightMapDTG.SetCellMovementColor(new Vector2(cell.currentCell.X, cell.currentCell.Y));
        }*/

        var spell = new Spell()
        {
            id = "0",
            range = 7,
            hitArea = new Vector2[] {new Vector2(0,0)}
        };
        ShowSpellRange(spell, position);
    }

    public void HideMovementRange()
    {
        fightMapDTG.ResetMovementCells();
    }

    public void ShowSpellRange(Spell spell, Vector2 position)
    {
        selectedSpell = spell;
        var cells = GetComponent<FightMapPathFinding>().FindSpellRange(position, spell.range);
        fightMapDTG.HighlightSpellRange(cells);
    }

    public void HideSpellRange()
    {
        if (selectedSpell != null)
        {
            selectedSpell = null;
            fightMapDTG.ResetSpellRangeCells();
        }
    }

    public void FindPath(Vector2 position)
    {
        if (mainFighterHandler.IsMovementPossible(position))
        {
           var path = GetComponent<FightMapPathFinding>().FindPath(mainFighter.GetFighter().Position, position);
            if(path != null && path.Count <= mainFighter.GetFighter().CurrentMovementPoint)
            {
                fightMapDTG.HighlightPath(path);
            }
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
