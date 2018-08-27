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

                if(selectedSpell != null && fightMapDTG.SpellImpactIsInRange(new Vector2(x, y)))
                {
                    Debug.Log("Use spell !");
                    HideSpellRange();
                    //mainFighterEmitter.UseSpell(selectedSpell)
                }
                else
                {
                    HideSpellRange();
                    if (fightMapDTG.GetHighlightedPath().Count != 0)
                    {
                        mainFighterEmitter.Move(fightMapDTG.GetHighlightedPath());
                    }
                }
            }else
            {
                HideSpellRange();
            }
        }
    }

    public void MouseOverCell(int x, int y)
    {
        if(fight.phase == 1)
        {
            if (selectedSpell != null && fightMapDTG.SpellImpactIsInRange(new Vector2(x, y)))
            {
                fightMapDTG.HighlightSpellImpact(selectedSpell.hitArea, new Vector2(x, y));
            }else if (fight.IsMainFighterTurn())
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
        if (selectedSpell != null) return;
        var cells = GetComponent<FightMapPathFinding>().FindRange(position, range);
        foreach (var cell in cells)
        {
            fightMapDTG.SetCellMovementColor(new Vector2(cell.currentCell.X, cell.currentCell.Y));
        }
    }

    public void HideMovementRange()
    {
        fightMapDTG.ResetMovementCells();
    }

    public void ShowSpellRange(Spell spell, Vector2 position)
    {
        if(fight.phase == 1)
        {
            fightMapDTG.BlockPathHighlighting(true);
            selectedSpell = spell;
            var cells = GetComponent<FightMapPathFinding>().FindSpellRange(position, spell.range);
            fightMapDTG.HighlightSpellRange(cells);
        }
    }

    public void ShowSpellRange(Spell spell)
    {
        ShowSpellRange(spell, mainFighter.GetFighter().Position);
    }


    public void HideSpellRange()
    {
        if (selectedSpell != null)
        {
            fightMapDTG.BlockPathHighlighting(false); 
            selectedSpell = null;
            fightMapDTG.ResetSpellRangeCells();
            fightMapDTG.ResetSpellImpact();
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
