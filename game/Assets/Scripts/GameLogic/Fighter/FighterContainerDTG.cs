using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContainerDTG : MonoBehaviour {

    public GameObject FighterGameObject;
    public GameObject MainFighterGameObject;
    private Dictionary<string, FighterDTG> players = new Dictionary<string, FighterDTG>();
    private MainFighterDTG mainFighter;


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
            playerGo.transform.parent = gameObject.transform;
            mainFighter = playerGo.GetComponent<MainFighterDTG>();
            mainFighter.SetFighter(fighter);
            mainFighter.InitFighter();
        } else
        {
            playerGo = Instantiate(FighterGameObject);
            playerGo.transform.parent = gameObject.transform;
            playerGo.GetComponent<FighterDTG>().SetFighter(fighter);
            playerGo.GetComponent<FighterDTG>().InitFighter();
            players.Add(fighter.Id, playerGo.GetComponent<FighterDTG>());
        }
        playerGo.name = fighter.Id;
        //playerGo.transform.parent = gameObject.transform;

    }

    public MainFighterDTG GetMainFighter()
    {
        return mainFighter;
    }

    public void MoveFighter(string id, List<Vector2> path)
    {

    }

    public void TeleportFighter(string id, Vector2 position)
    {

    }

}
