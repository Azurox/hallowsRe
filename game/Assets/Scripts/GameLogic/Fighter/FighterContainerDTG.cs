using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContainerDTG : MonoBehaviour {

    public GameObject FighterGameObject;
    public GameObject MainFighterGameObject;
    private FightUIManager FightUIManager;
    private FightMapDTG FightMapDTG;
    private Dictionary<string, FighterDTG> fighters = new Dictionary<string, FighterDTG>();
    private MainFighterDTG mainFighter;


    public void Startup(FightUIManager fightUIManager, FightMapDTG fightMapDTG)
    {
        FightUIManager = fightUIManager;
        FightMapDTG = fightMapDTG;
    }

    public void Init(List<Fighter> fighters)
    {
        foreach (var fighter in fighters)
        {
            SpawnFighter(fighter);
        }
    }

    public void SpawnFighter(Fighter fighter)
    {
        GameObject playerGo;
        if (fighter.IsMainPlayer)
        {
            playerGo = Instantiate(MainFighterGameObject);
            playerGo.transform.parent = gameObject.transform; // Need to attribute parent as fast as possible otherwhise the Collider is not working ?
            mainFighter = playerGo.GetComponent<MainFighterDTG>();
            mainFighter.SetFighter(fighter);
            mainFighter.InitFighter();
            GetComponent<MainFighterHandler>().Init(mainFighter);
        } else
        {
            playerGo = Instantiate(FighterGameObject);
            playerGo.transform.parent = gameObject.transform; // Same.
            playerGo.GetComponent<FighterDTG>().SetFighter(fighter);
            playerGo.GetComponent<FighterDTG>().InitFighter();
            fighters.Add(fighter.Id, playerGo.GetComponent<FighterDTG>());
        }
        playerGo.name = fighter.Id;
    }

    public MainFighterDTG GetMainFighter()
    {
        return mainFighter;
    }

    public void MoveFighter(string id, List<Vector2> path)
    {
        if (fighters.ContainsKey(id))
        {
            FighterDTG fighter = fighters[id];
            FightMapDTG.SetCellAvailability(fighter.GetFighter().Position, false);

            fighter.GetComponent<Movable>().TakePath(new Vector2(fighter.gameObject.transform.position.x, fighter.gameObject.transform.position.z) ,path, (x, y)=>
            {
                fighter.GetFighter().CurrentMovementPoint--;
                fighter.GetFighter().Position = new Vector2(x, y);
            });

            FightMapDTG.SetCellAvailability(fighter.GetFighter().Position, true);
        }
    }

    public void MoveMainFighter(List<Vector2> path)
    {
        var fighter = mainFighter.GetFighter();
        FightMapDTG.SetCellAvailability(fighter.Position, false);
        FightMapDTG.ResetPathCells();
        FightMapDTG.BlockPathHighlighting(true);
        mainFighter.GetComponent<Movable>().TakePath(new Vector2(mainFighter.transform.position.x, mainFighter.gameObject.transform.position.z), path, (x, y) =>
        {
            fighter.CurrentMovementPoint--;
            fighter.Position = new Vector2(x, y);

            if (path.Count == 0)
            {
                FightMapDTG.BlockPathHighlighting(false);
                GetComponent<MainFighterHandler>().MainFighterMoved();
                FightMapDTG.SetCellAvailability(fighter.Position, true);
            }
        });
    }

    public void TeleportFighter(string id, Vector2 position)
    {
        if (fighters.ContainsKey(id))
        {
            FighterDTG fighter = fighters[id];
            fighter.SetPosition((int)position.x, (int)position.y);
        }
    }


    public void TeleportMainFighter(Vector2 position)
    {
        mainFighter.SetPosition((int)position.x, (int)position.y);
    }

    public void FocusFighter(Fighter fighter)
    {
        FightUIManager.ShowFighterStats(fighter);
    }

}
